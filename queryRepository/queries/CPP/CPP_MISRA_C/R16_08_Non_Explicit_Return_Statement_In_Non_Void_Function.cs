/*
MISRA C RULE 16-8
------------------------------
This query searches for functions with non-void return type and an exit path without an explicit 'return' statement

	The Example below shows code with vulnerability: 

int foo ( int param )
{
   if ( param > 5 )
   {
      return ( param );
   }
}

*/

//finds all methods that have non void return type but return void.
CxList returnStmt = All.FindByType(typeof(ReturnStmt));
CxList emptyReturn = returnStmt - All.FindByFathers(returnStmt).GetFathers();
CxList returnType = All.FindByType(typeof(TypeRef)).FindByFathers(emptyReturn.GetAncOfType(typeof(MethodDecl)));
CxList voidRT = returnType.FindByShortName("void");
CxList nonVoidRT = returnType - voidRT;
result = nonVoidRT;


//finds methods that have non void return type but don't have return statements:
CxList allMethodDecl = All.FindByType(typeof(MethodDecl));
//remove all definitions
CxList declared = All.FindByFathers(allMethodDecl).FindByType(typeof(StatementCollection)).GetFathers();
//get non those who have non void return types:
returnType = All.FindByType(typeof(TypeRef)).FindByFathers(declared);
nonVoidRT = returnType - returnType.FindByShortName("void");
CxList backToMeth = nonVoidRT.GetFathers();

foreach(CxList method in backToMeth)
{
	CxList foundRetStmt = returnStmt.GetByAncs(method);
	if(foundRetStmt.Count == 0)
	{
		result.Add(method);
	}
}
result -= allMethodDecl.FindByShortName("main");