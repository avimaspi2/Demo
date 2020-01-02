/*
MISRA C RULE 16-4
------------------------------
This query searches for prototypes (such as function declarations) and definitions of same function
with different identifiers for the paramaters

	The Example below shows code with vulnerability: 

static void func123a ( int ) ;

static void func123a ( int signum ){
...
}

*/

CxList methodDecls = All.FindByType(typeof(MethodDecl));
CxList paramaters = All.GetParameters(methodDecls);

// go over all method declarations or definitions
foreach(CxList curMethodDecl in methodDecls)
{
	CxList curParams = paramaters.GetParameters(curMethodDecl);

	// compare to all same name declarations/definitions (except self)
	CxList sameNameDecls = methodDecls.FindByName(curMethodDecl);
	sameNameDecls -= curMethodDecl;

	foreach(CxList compMethodDecl in sameNameDecls)
	{		
		CxList compParams = paramaters.GetParameters(compMethodDecl);
		
		// compare paramater identifiers by order
		if (curParams.Count != compParams.Count)
			result.Add(curMethodDecl + compMethodDecl);
		else
			for (int i = 0;i < curParams.Count;i++){ 
				ParamDecl cur = curParams.data.GetByIndex(i) as ParamDecl;
				ParamDecl comp = compParams.data.GetByIndex(i) as ParamDecl;
				if (String.Compare(cur.Name, comp.Name) != 0){
					result.Add(curMethodDecl + compMethodDecl);
					break;	
				}
			}
	}
}