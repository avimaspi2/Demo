result = Find_Connection_Strings(new string[] {"adodb*"}, 
	new string[]{"NewADOConnection*"});

result.Add(Find_Connection_Strings(new string[] {"dbo*"}, 
	new string[]{"dsn_connect*"}));

result.Add(Find_Connection_Strings(new string[] {"django.db*"}, 
	new string[]{"connection*"}));

result.Add(Find_Connection_Strings(new string[] {"pymongo*"}, 
	new string[]{"Connection*", "MongoClient"}));

result.Add(Find_Connection_Strings(new string[] {"pymssql*", "_mssql*"},
	new string[]{"connect*"}));

result.Add(Find_Connection_Strings(new string[] {"MySQLdb","mysql","oursql","pymysql","cymysql"}, 
	new string[]{"connect*","connector.connect"}));
	
result.Add(Find_Connection_Strings(new string[] {"pyodbc","pypyodbc*","odbtpapi","odbtp","ceODBC"}, 
	new string[]{"connect*","win_connect_mdb", "Connection"}));

result.Add(Find_Connection_Strings(new string[] {"jaydebeapi*"}, 
	new string[]{"connect*"}));

result.Add(Find_Connection_Strings(new string[] {"cx_Oracle*"}, 
	new string[]{"connect*"}));
	
result.Add(Find_Connection_Strings(new string[] {"peewee*"}, 
	new string[]{"SqliteDatabase","MySQLDatabase","PostgresqlDatabase","Database"}));
	
result.Add(	Find_Connection_Strings(new string[] {"psycopg2*", "pg","pgdb","PgSQL","pyPgSQL","postgresql",
	"postgresql.driver","pg8000"}, new string[]{"connect","open","DB"}));

result.Add(	Find_Connection_Strings(new string[] {"sdb*", "sapdb*"},
	new string[]{"connect*"}));
	
result.Add(Find_Connection_Strings(new string[] {"sqlalchemy*"}, 
	new string[]{"create_engine","declarative_base","sessionmaker","create_session", "mapper"}));
	
result.Add(Find_Connection_Strings(new string[] {"sqlanydb*"}, 
	new string[]{"connect*"}));

result.Add(Find_Connection_Strings(new string[] {"sqlite3*"}, 
	new string[]{"connect*"}));

result.Add(Find_Connection_Strings(new string[] {"sqlobject*"}, 
	new string[]{"connectionForURI*"}));

result.Add(Find_Connection_Strings(new string[] {"sybase","Sybase"}, 
	new string[]{"connect*"}));