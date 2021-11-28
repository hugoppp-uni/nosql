﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Content;
using backend.Models;
using Cassandra;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace backend.Services
{

    public class CassandraCityService : ICityService
    {
        private readonly ISession _cassandra;

        public CassandraCityService(ISession cassandra, ILogger<CassandraCityService> logger)
        {
            _cassandra = cassandra;
            ImportToCassandra(_cassandra);
            logger.LogInformation("Imported data to cassandra");
        }

        public City? GetCityFromZip(string zip)
        {
            Row? row = _cassandra
                .Execute($"SELECT * from CITY where zip='{zip}'")
                .FirstOrDefault();

            if (row is null)
                return null;

            return new City() { Name = row.GetValue<string>("name"), State = row.GetValue<string>("state") };
        }

        public IEnumerable<string> GetZipsFromCity(string city)
        {
            RowSet rowSet = _cassandra.Execute($"SELECT zip from CITY where name='{city}' ALLOW FILTERING");
            return rowSet.Select(x => x.GetValue<string>("zip"));
        }

        private static void ImportToCassandra(ISession cassandra)
        {
            cassandra.Execute(
                "CREATE TABLE CITY (zip text, name text, state text, soccer text, PRIMARY KEY(zip))"
            );

            IEnumerable<Task<RowSet>> insertTasks =
                File.ReadAllLines(ContentPath.PlzData)
                    .Select(line => JsonSerializer.Deserialize<JsonElement>(line))
                    .Select(json => $"INSERT INTO CITY (zip, name, state) " +
                                    $"VALUES ('{json.GetString("_id")}', '{json.GetString("city")}', '{json.GetString("state")}')")
                    .Select(query => cassandra.ExecuteAsync(new SimpleStatement(query)));

            Task.WhenAll(insertTasks);
        }

    }
}
