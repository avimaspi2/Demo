CxList db = Find_DB_In();//Get DB IN methods
CxList strings = Find_Strings();//Get Strings

CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase); //Find update strings
write.Add(strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase));//Find delete strings
write.Add(strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase));//Find insert strings

CxList relevantDB = db.DataInfluencedBy(write);
CxList cakeDb = Find_Cake_DB_In();
cakeDb -= Find_Cake_DB_In_Query();
relevantDB.Add(cakeDb);
result.Add(relevantDB);