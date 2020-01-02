//DWR framework prevents javascript hijacking
CxList dwrFramework = All.FindByName("*dwr.util*", true); 

CxList CleanAJAXFramework = All.NewCxList();
// we'll add other frameworks that take care of javascript hijacking to this lis
CleanAJAXFramework.Add(dwrFramework);

if (CleanAJAXFramework.Count == 0)
{
	CxList names = All.NewCxList();
	names.Add(All.FindByName("*select*", false));
	names.Add(All.FindByName("*exec*", false));
	
	CxList db = Find_DB_Out().DataInfluencedBy(names);
	
	CxList jason = All.FindByTypes(new string[]{"JsonObject","JsonValue","JsonArray","JsonParser"}, false);
	result = jason.DataInfluencedBy(db); 
}