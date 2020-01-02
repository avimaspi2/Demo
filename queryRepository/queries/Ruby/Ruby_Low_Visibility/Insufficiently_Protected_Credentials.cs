CxList AllPSW = Find_Passwords();

AllPSW = AllPSW - Find_Methods();

CxList AllDB = Find_DB_Methods();
CxList DBOut = Find_DB();

foreach (CxList db in DBOut)
{
	result.Add(AllPSW.InfluencedByAndNotSanitized(db, AllDB - db));
}