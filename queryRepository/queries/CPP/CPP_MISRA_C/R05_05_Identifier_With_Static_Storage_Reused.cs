/*
MISRA C RULE 5-5
------------------------------
This query searches for names of identifiers with static storage that are reused anywhere in the code

	The Example below shows code with vulnerability: 

static int dummy;
unsigned dummy;

*/


//all declarations
CxList allDecl = Find_All_Declarators();
//get all static declarations
Modifiers mod = new Modifiers();
mod = Dom.Modifiers.Static;
CxList fd = All.FindByType(typeof(FieldDecl));
CxList dcltr = Find_All_Declarators();
CxList staticFields = All.FindByFieldAttributes(mod) - All.FindByType(typeof(MethodDecl));
staticFields = (allDecl + fd).GetByAncs(staticFields);
CxList classDecl = All.FindByType(typeof(ClassDecl));


//compare names of all declarations

foreach(CxList stat in staticFields)
{
	
	CSharpGraph statName = stat.data.GetByIndex(0) as CSharpGraph;
	CxList totalApp = ((allDecl).FindByShortName(statName.ShortName));
	if(totalApp.Count > 1)
	{
		result.Add(totalApp);
	}
}


//for all non- members functions
CxList allMethods = All.FindByType(typeof(MethodDecl));
//remove all defintions
CxList stmtCol = All.FindByType(typeof(StatementCollection));
foreach(CxList cur in allMethods)
{
	CxList myClass = classDecl.GetClass(cur);
	if (stmtCol.FindByFathers(cur).Count == 0)
	{
		allMethods -= cur;
	}
}
//get all static method declarations
CxList staticMethods = allMethods.FindByType(typeof(MethodDecl));
staticMethods = staticMethods.FindByFieldAttributes(mod);

//compare their names
foreach(CxList stat in staticMethods)
{	
	CxList totalApp = (allMethods).FindByShortName(stat);
	if(totalApp.Count > 1)
	{
		result.Add(totalApp);
	}
}