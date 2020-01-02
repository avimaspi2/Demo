/*
MISRA C RULE 16-9
------------------------------
This query searches for function identifiers not followed by parenthesised parameter list or preceded by '&'

	The Example below shows code with vulnerability: 

int foo();
int boo();

if (foo == boo){
...
}

*/

CxList methodDecls = All.FindByType(typeof(MethodDecl));
CxList addresses = All.FindByType(typeof(UnaryExpr)).FindByName("Address");
CxList references = All.FindByType(typeof(UnknownReference)).FindAllReferences(methodDecls);
references -= references.FindByFathers(references.GetFathers().FindByShortName("Address"));
result = references;