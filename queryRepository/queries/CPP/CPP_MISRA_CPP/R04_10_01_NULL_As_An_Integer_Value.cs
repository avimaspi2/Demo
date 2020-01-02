/*
 MISRA CPP RULE 4-10-1
 ------------------
  NULL shall not be used as an integer value.
  
  The Example below shows code with vulnerability: 

 				#include <cstddef>
				void f1 (int a);
				void f2 (int * a);
				void f3 ()
				{
					f1 (NULL) //Non-compliant, NULL used as an integer.
					f2 (NULL) //Compliant
				}

*/

CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList paramDecl = All.FindByType(typeof(ParamDecl)); 
CxList pointers = paramDecl.FindByRegex(@"\w\s*\*\s*\w");
CxList nullParams = All.FindByType(typeof(Param)).FindByShortName("null");
nullParams = nullParams.GetByAncs(All.FindByType(typeof(MethodInvokeExpr)));
CxList nullMethInvoke = nullParams.GetAncOfType(typeof(MethodInvokeExpr));
CxList nullMethDecl = methodDecl.FindDefinition(nullMethInvoke);

//Get all int typerefs (also typedef'd refs)
CxList typedefs = All.FindByName("CX_TYPEDEF").FindByType(typeof(StringLiteral));
typedefs = typedefs.GetAncOfType(typeof(VariableDeclStmt));
CxList tpr = All.FindByType(typeof(TypeRef));
CxList intTypeRefs = All.NewCxList();

foreach(CxList nf in typedefs)
{
	CSharpGraph g = nf.data.GetByIndex(0) as CSharpGraph;
	if(g.TypeName == "int" ||
		g.TypeName == "short" ||
		g.TypeName == "long")
	{
		intTypeRefs.Add(tpr.FindByShortName(g.ShortName));
	}
}

intTypeRefs.Add(tpr.FindByName("int") + 
	tpr.FindByName("short") +
	tpr.FindByName("long"));

foreach(CxList nullInvoke in nullMethInvoke)
{
	int paramCounter = 0;
	LinePragma lp = ((CSharpGraph) nullInvoke.data.GetByIndex(0)).LinePragma;
	CxList curMethDecl = nullMethDecl.FindDefinition(nullInvoke);
	if (curMethDecl.Count == 0) {//Definition not found.
		continue;
	}
	LinePragma clp = ((CSharpGraph) curMethDecl.data.GetByIndex(0)).LinePragma;
	foreach(CxList curParamDecl in All.FindByPosition(clp.FileName, clp.Line).GetParameters(curMethDecl))
	{
		CSharpGraph g = curParamDecl.data.GetByIndex(0) as CSharpGraph;
		if (pointers.FindById(g.NodeId).Count > 0) {
			paramCounter++;
			continue;
		}
		CxList curParam = All.FindByPosition(lp.FileName, lp.Line).GetParameters(nullInvoke, paramCounter);
		curParam = curParam.FindByType(typeof(Param));
		if (curParam.FindByShortName("null").Count > 0 &&
		intTypeRefs.FindById(((ParamDecl) g).Type.NodeId).Count > 0) {
			result.Add(curParam);
		}
		paramCounter++;
	}
	
}