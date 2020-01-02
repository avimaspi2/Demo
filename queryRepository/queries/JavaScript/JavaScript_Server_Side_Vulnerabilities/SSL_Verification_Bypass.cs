/*looks for rejectUnauthorized set to false in Node.JS*/
/*server side indicators */
CxList imports = Find_Import() - XS_Find_All();
List <string> libs = new List<string>{"http", "fs"};
CxList serverSide = All.NewCxList();

foreach(CxList imp in imports){
	Import import = imp.TryGetCSharpGraph<Import>();
	if(libs.Contains(import.ImportedFilename)){
		serverSide.Add(imp);	
	}
}

if(serverSide.Count > 0)
{
	CxList reject = All.FindByShortName("rejectUnauthorized");	
	CxList falseBoolean = All.FindByShortName("false").FindByType(typeof(BooleanLiteral));
	result = reject.DataInfluencedBy(falseBoolean);	
}