/* MISRA CPP RULE 2-10-5
 ------------------------------
 This query checks if there is a reuse of the name of a non-member static object or  static function
 
 The Example below shows code with vulnerability: 
     
 
     static char x='a';

     static int  foo()   
     {
         int x=9;          //non-compliant - reuse of x
     }
         
     class A   
     {
		    void foo()	   //non-compliant - reuse of foo
          {        
				some code
          }
     } 

*/



//all declarations
CxList allDecl = Find_All_Declarators();
//get all static declarations
Modifiers mod = new Modifiers();
mod = Dom.Modifiers.Static;
CxList fd = All.FindByType(typeof(FieldDecl));
CxList dcltr = Find_All_Declarators();

CxList staticFields = All.FindByFieldAttributes(mod) - All.FindByType(typeof(MethodDecl));
staticFields = (dcltr + fd).GetByAncs(staticFields);

CxList classDecl = All.FindByType(typeof(ClassDecl));
//get non- memebers declarations
foreach(CxList cur in staticFields)
{
	CxList myClass = classDecl.GetClass(cur);
	CSharpGraph d = myClass.data.GetByIndex(0) as CSharpGraph;
	if (d!=null && !d.ShortName.StartsWith("checkmarx"))
	{
		staticFields -= cur;
	}
}


//compare names of all declarations

foreach(CxList stat in staticFields)
{
	CxList totalApp = All.NewCxList();
	CSharpGraph statName = stat.data.GetByIndex(0) as CSharpGraph;
	if(statName != null)
	{
		totalApp = ((allDecl).FindByShortName(statName.ShortName));
	}
	if(totalApp.Count > 1)
	{
		result.Add(totalApp);
	}
}


//for all non- members functions
CxList allMethods = All.FindByType(typeof(MethodDecl));
//get all non-members
CxList stmtCol = All.FindByType(typeof(StatementCollection));
foreach(CxList cur in allMethods)
{
	CxList myClass = classDecl.GetClass(cur);
	CSharpGraph d = myClass.data.GetByIndex(0) as CSharpGraph;
	if (d!=null && (!d.ShortName.StartsWith("checkmarx") || stmtCol.FindByFathers(cur).Count == 0))
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
	CxList totalApp = All.NewCxList();
	CSharpGraph statName = stat.data.GetByIndex(0) as CSharpGraph;
	if(statName != null)
	{
		 totalApp = (allMethods).FindByShortName(statName.ShortName);
	}
	if(totalApp.Count > 1)
	{
		result.Add(totalApp);
	}
}