result.Add(Find_Require("db-mysql"));
result.Add(Find_Require("mysql"));
result.Add(Find_Require("node-sqlserver"));
result.Add(Find_Require("pg"));
result.Add(Find_Require("cassandra-driver"));
result.Add(Find_Require("oracledb"));
result.Add(Find_Require("mongodb"));

CxList imports = Find_Import();
result *= imports;