CxList methods = Find_Methods();
CxList SQLiteDB = methods.FindByMemberAccess("SqLiteDataBase.*");

CxList db =	SQLiteDB.FindByMemberAccess("SqLiteDataBase.delete*");
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.query*"));
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.execSQL*"));
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.insert*"));
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.compileStatement*"));
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.rawQuery*"));
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.update*"));
db.Add(SQLiteDB.FindByMemberAccess("SqLiteDataBase.replace*"));

result = All.GetParameters(db);