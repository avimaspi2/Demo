//DWR framework prevents javascript hijacking
CxList dwrFramework= All.FindByName("*dwr.util*",true); 

CxList CleanAJAXFramework=dwrFramework;// we'll add other frameworks that take care of javascript hijacking to this lis

if (CleanAJAXFramework.Count==0)
{
	CxList db=Find_DB_Out().DataInfluencedBy(All.FindByName("*select*",false)+All.FindByName("*exec*",false));
	CxList jason=All.FindByName("*json*",false);
	jason=jason.DataInfluencedBy(db);
	result= jason;
}