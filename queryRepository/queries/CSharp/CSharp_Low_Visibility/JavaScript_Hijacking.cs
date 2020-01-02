// DWR is not relevant for C#, therefore it was removed

CxList db  =  Find_DB_Out().DataInfluencedBy(All.FindByName("*select*",  false)  +  All.FindByName("*exec*",  false));
CxList jason = All.FindByName("*JSON*", false);
jason = jason.DataInfluencedBy(db);
result = jason;