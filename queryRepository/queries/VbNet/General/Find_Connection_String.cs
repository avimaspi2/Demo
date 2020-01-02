string[] types = { 
	"*oledbconnection",
	"*odbcconnection",
	"*sqlconnection",
	"*oracleconnection",
	"*db2connection",
	"*.entityconnection",
	"*database",
	"*oracledatabase",
	"*sqldatabase",
	"*genericdatabase",
	"*datacontext",
	"*connectionstringbuilder"	};
 

CxList tmpResult = 
	All.FindByTypes(types, false);

//add inheritance from the types
CxList cd = All.FindByType(typeof(ClassDecl));
CxList inheritsFromBuilder = All.NewCxList();
foreach(string type in types)
{
	inheritsFromBuilder.Add(cd.InheritsFrom(type));
}

ArrayList childTypesList = new ArrayList();
foreach(CxList child in inheritsFromBuilder)
{
	string childTypeName = child.GetName();
	if(!childTypeName.Equals(String.Empty))
	{
		childTypesList.Add(childTypeName);
	}
	
}
result.Add(All.FindByTypes((String[]) childTypesList.ToArray(typeof( string ))));
result.Add(tmpResult);