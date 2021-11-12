[TOC]
# NoSQL Praktikum - Alexander Könemann, Hugo Protsch

# NoSQl Termin/Blatt 2

## Aufgabe 4: PLZ-API Redis
Die Aufgabe wurde in Form einer REST-API gelöst.
Die Dokumentation dieser ist unter anderem mithilfe von Swagger auf Port `31474` (`http://localhost:31474/swagger`) vorzufinden.

Es existieren zwei Endpoints, welche im `ZipDataController` definiert sind:
1. `GET /city/{zipCode}` gibt die den Stadtnamen und Staat der Postleitzahl zurück.
2. `GET /zip/{cityName}` gibt eine Liste aller Postleitzahlen zurück, dessen Stadtname `cityName` entspricht.

In der Redis Datenbank werden dafür die folgenden Daten importiert:

1. Key: `{zip}.state` Value: Der Staat, in dem sich die Stadt mit der Postleitzahl `{zip}` befindet
2. Key: `{zip}.city` Value: Der Name der Stadt mit der Postleitzahl `{zip}`
3. Key: `{city}.zip` Value: Eine Liste aller Postleitzahlen mit dem Stadtnamen `{city}`

## Aufgabe 5: Absolvierte Module
1. Start Neo4j (graph db):
```bash
docker run \
    --name our-neo4j \
    -p7474:7474 -p7687:7687 \
    --env NEO4J_AUTH=neo4j/test \
    neo4j:latest
```
Browser runs on localhost:7474
```
username: neo4j pw: test
```
2. Run script of `ModulesAlexander.cypher` / `ModulesHugo.cypher` file in Neo4j browser Interface

### Output Graph Alexander

![ModulesGraphOutputAlexander](Aufgabe5-graph-db/ModulesGraphOutputAlexander.png)

### Output Graph Hugo

![ModulesGraphOutputHugo](Aufgabe5-graph-db/ModulesGraphOutputHugo.png)

- Aufgabe 5b 1.): Welche Module sind für NoSQL/BigData nützlich?
```cypher
MATCH (c1:Course)-[:USED_IN]->(c2:Course) WHERE c2.name = "NOSQL" RETURN DISTINCT c1.name
```
- Aufgabe 5b 2.): Welche Module wurden bisher im Studium nicht wieder genutzt? 
Anders formuliert: Welche Knoten haben keinen Nachfolgeknoten?
```cypher
MATCH (c1:Course) WHERE NOT exists((c1)-[:USED_IN]->()) RETURN DISTINCT c1.name`
```

## Aufgabe 6 Conceptnet
1. Open bash on container and remove folders in data folder:
```bash
docker exec -it our-neo4j  /bin/bash
```
2. Import of data via docker cp command:
```bash
docker cp /Users/alexander.koenemann/IdeaProjects/NoSQLWP/nosqlHugoAlex/Aufgabe6-graph-db/neo4j-v4-data/. our-neo4j:data/
```
3. Query:
```cypher
MATCH (n {id : "/c/en/baseball"})-[r:IsA]-(result) RETURN result.id
````

# Termin/Blatt 3
## Aufgabe 7: Mongo DB API
Die Aufgabe wurde in Form einer REST-API gelöst. Die Dokumentation dieser ist unter anderem mithilfe von Swagger auf
Port `31474` (`http://localhost:31474/swagger/index.html`) vorzufinden.
### Vorbereitung

- Starten der shell:
  - `docker compose up` startet die Mongo, Redis und Cassandra DB container.

Es existieren zwei Endpoints, welche im `PlzDataController` definiert sind:

1. `GET /city/{zipCode}` gibt die den Stadtnamen und Staat der Postleitzahl zurück.
2. `GET /zip/{cityName}` gibt eine Liste aller Postleitzahlen zurück, dessen Stadtname `cityName` entspricht.

In der Mongo Datenbank werden dafür die folgenden Daten aus der 'plz.data' importiert:

1. Key: `{zip}.state` Value: Der Staat, in dem sich die Stadt mit der Postleitzahl `{zip}` befindet
2. Key: `{zip}.city` Value: Der Name der Stadt mit der Postleitzahl `{zip}`
3. Key: `{city}.zip` Value: Eine Liste aller Postleitzahlen mit dem Stadtnamen `{city}`

d) Benchmarktest, LoC,
Arbeitszeit:
<!-- TABLE_GENERATE_START -->

| Database      | Mean          | StdDev        | Loc           | Arbeitszeit
| ------------- | ------------- | ------------- | ------------- | -------------
| BenchRedis    | 6.272 ms      | 0.1013        | 95            | \> Mongo, da nicht alles in einem Dokument gespeichert werden kann
| BenchMongo    | 89.854 ms     | 0.8986        | 79            | \< Redis, keine Json Format verinfacht Entwicklung

<!-- TABLE_GENERATE_END -->

## Aufgabe 8: Sinn des Lebens
### Vorbereitung
- Starten der shell:
   - `docker exec -it {container name} bash`
   - `mongo`
- Löschen der Daten: 
  - `db.getCollection("fussball").drop()`
- Zählen der Daten:
  - `db.getCollection("fussball").find().count()`

a) Einfügen der Daten
```javascript
db.fussball.insertMany([
    {name: 'HSV', gruendung: new Date(1888, 09, 29), farben: ['weiss', 'rot'], Tabellenplatz: 17, nike: 'n'},
    {name: 'Dortmund', gruendung: new Date(1909, 12, 19), farben: ['gelb', 'schwarz'], Tabellenplatz: 16, nike: 'n'},
    {name: 'Schalke', gruendung: new Date(1904, 5, 4), farben: ['blau'], Tabellenplatz: 15, nike: 'n'},
    {name: 'Paderborn', gruendung: new Date(1907, 8, 14), farben:['blau', 'weiss', ], Tabellenplatz:14, nike:'n', },
    {name: 'Hertha', gruendung: new Date(1892, 7, 25), farben: ['blau', 'weiss'], Tabellenplatz: 13, nike: 'j'},
    {name: 'Augsburg', gruendung: new Date(1907, 8, 8), farben: ['rot', 'weiss'], Tabellenplatz: 12,  nike: 'j'},
    {name: 'Pauli', gruendung: new Date(1910, 5, 15), farben: ['braun', 'weiss'], Tabellenplatz: 11, nike: 'n'},
    {name: 'Gladbach', gruendung: new Date(1900, 8,1), farben: ['schwarz', 'weiss', 'gruen'], Tabellenplatz: 10, nike: 'n'},
    {name: 'Frankfurt', gruendung: new Date(1899, 3, 8), farben: ['rot', 'schwarz', 'weiss'], Tabellenplatz: 9, nike: 'j'},
    {name: 'Leverkusen', gruendung: new Date(1904, 11, 20, 16, 15), farben: ['rot', 'schwarz'], Tabellenplatz: 8, nike: 'n'},
    {name: 'Stuttgart', gruendung: new Date(1893, 9, 9 ), farben: ['rot', 'weiss'], Tabellenplatz: 7, nike: 'n'},
    {name: 'Werder', gruendung: new Date(1899,2,4), farben: ['gruen','weiss'], Tabellenplatz: 6, nike: 'j'}
]);
```
### Find Abfragen
1. alle Vereine, mit Namen "Augsburg"
```javascript
db.getCollection("fussball").find({ "name" : "Augsburg" })
```
2. alle Nike-Vereine, welche schwarz als mindestens eine Vereinsfarbe haben
```javascript
db.getCollection("fussball").find({ "nike" : "j", "farben" : { $all : [ "schwarz" ] } })
```
3. alle Nike-Vereine, welche weiss und grün als Vereinsfarbe haben
```javascript
db.getCollection("fussball").find({ "nike" : "j", "farben" : { $all : [ "weiss", "gruen" ] } })
```
4. alle Nike-Vereine, welche weiss oder grün als Vereinsfarbe haben
```javascript
 db.getCollection("fussball").find( 
     { "nike" : "j",
         $or : [
             { "farben" : { $all : [ "gruen" ] } },
             { "farben" : { $all : [ "weiss" ] } } 
         ] 
     } 
 );
```
5. den Verein mit dem höchsten Tabellenplatz
```javascript
db.getCollection("fussball").find().sort({ Tabellenplatz : 1 }).limit(1)
```
6. alle Vereine, die nicht auf einem Abstiegsplatz stehen
```javascript
db.getCollection("fussball").find({ Tabellenplatz : { $lt : 15 } })
```
7. Erstellen Sie eine beliebige andere sinnvolle Abfrage und unterdrücken Sie dabei die Ausgabe des \_id Feldes
```javascript
db.getCollection("fussball").find({ gruendung : { $gt : new Date(1900, 1, 1) } }, { _id : 0 })
```

### Update Abfragen
Führen sie folgende Änderungsoperation aus:
```javascript
db.fussball.update({ name : 'Augsburg' }, { Tabellenplatz : 1 })
```
Was beobachten sie als Ergebnis?
  - `{ "_id" : ObjectId("618bfaafa0817d590df179df"), "Tabellenplatz" : 1 }`
  Das komplette Objekt wird durch das übergebene (`"Tabellenplatz" : 1`) ersetzt. Die id bleibt jedoch gleich.

1. Ändern sie den Tabellenplatz von Leverkusen auf 2
```javascript
db.fussball.update({ name : "Leverkusen" }, { $set : { Tabellenplatz : 2 } })
```
2. Werder soll um einen Tabellenplatz nach vorne gebracht werden
```javascript
db.fussball.update({ name : "Leverkusen" }, { $inc : { Tabellenplatz : - 1 } })
```
3. Ergänzen sie für den HSV ein Attribut „abgestiegen“ mit einem sinnvollen Wert
```javascript
db.fussball.update({ name : "HSV" }, { $set : { "abgestiegen" : 1 } })
```
4. Ergänzen sie für alle Vereine, deren Vereinsfarbe weiss enthält, ein Attribut "Waschtemperatur" mit dem Wert 90.
```javascript
db.fussball.update({ "farben" : { $all : ["weiss"] } }, { $set : { "Waschtemperatur" : 90 } }, { multi : true } )
```
## Aufgabe 9: Installieren Wide-Column-Database
- Starten der shell:
  - `docker compose up` startet die Mongo, Redis und Cassandra DB container.
