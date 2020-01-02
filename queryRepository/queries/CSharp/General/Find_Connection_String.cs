string[] types = { 
   
	"*.DbConnection",
	"*.OleDbConnection",
	"*.OdbcConnection",
	"*.SqlConnection",
	"*.OracleConnection",
	"*.DB2Connection",
	"*.EntityConnection",
	
	"DbConnection",
	"OleDbConnection",
	"OdbcConnection",
	"SqlConnection",
	"OracleConnection",
	"DB2Connection",
	"EntityConnection",
	
	// EntLib (connectionString - first parameter) 
	"*.Database",
	"*.OracleDatabase",
	"*.SqlDatabase",
	"*.GenericDatabase", 

	"Database",
	"OracleDatabase",
	"SqlDatabase",
	"GenericDatabase",
	
	"*DataContext",
	"*ConnectionStringBuilder"	
	};



CxList tmpResult = 
	All.FindByTypes(types);

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