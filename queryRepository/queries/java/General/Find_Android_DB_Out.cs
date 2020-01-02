CxList methods = Find_Methods();
CxList SQLiteDB = methods.FindByMemberAccess("SqLiteDataBase.*");
CxList SQLiteStatement = methods.FindByMemberAccess("SQLiteStatement.*");

result = SQLiteDB.FindByMemberAccess("SqLiteDataBase.query*");
result.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.execSQL*"));
result.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.rawQuery*"));
result.Add(SQLiteStatement.FindByMemberAccess("SQLiteStatement.simpleQueryForString"));
result.Add(SQLiteStatement.FindByMemberAccess("SQLiteStatement.simpleQueryForBlobFileDescriptor"));
result.Add(SQLiteStatement.FindByMemberAccess("SQLiteStatement.simpleQueryForLong"));
result.Add(SQLiteStatement.FindByMemberAccess("SQLiteStatement.execute*"));