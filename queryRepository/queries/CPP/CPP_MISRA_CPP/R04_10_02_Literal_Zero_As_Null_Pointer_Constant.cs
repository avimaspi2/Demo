/*
 MISRA CPP RULE 4-10-2
 ------------------
  Literal zero (0) shall not be used as null-pointer-constant.
  
  The Example below shows code with vulnerability: 

 				#include <cstddef>
				void f1 (int a);
				void f2 (int * a);
				void f3 ()
				{
					f1 (0) //Compliant
					f2 (0) //Non-compliant
				}

*/
CxList methodDecl = All.FindByType(typeof(MethodDecl));

CxList paramDecl = All.FindByType(typeof(ParamDecl));
CxList tr = All.FindByType(typeof(TypeRef));
CxList ptr = All.FindByRegex(@"([^(]\s*(\*\s*)+\w)+?");
CxList pdc = All.FindByFathers(ptr.FindByType(typeof(ParamDeclCollection)));
CxList theMeth = pdc.GetAncOfType(typeof(MethodDecl));
CxList pointers = ptr * (paramDecl + tr);
pointers = paramDecl.GetParameters(theMeth, 0) + (pointers.FindByType(typeof(TypeRef)).GetAncOfType(typeof(ParamDecl)) + (pointers * paramDecl));
CxList mtd = pointers.GetAncOfType(typeof(MethodDecl));

CxList methInv = All.FindByType(typeof(MethodInvokeExpr));
CxList zero = All.FindByShortName("0").FindByType(typeof(IntegerLiteral));




int i = 0;
CxList solution = All.NewCxList();
foreach(CxList metDecl in mtd){
	CxList prm = paramDecl.GetParameters(metDecl);
	foreach(CxList paramm in prm){		
		CSharpGraph g = paramm.data.GetByIndex(0) as CSharpGraph;
		if (pointers.FindById(g.NodeId).Count > 0)
		{				
			CxList reff = methInv.FindAllReferences(metDecl);
			foreach (CxList invoke in reff){
				if (zero.GetParameters(invoke, i).Count > 0){
				
					solution.Add(prm.GetParameters(mtd.FindDefinition(invoke), i));
					
				}
			}
		}
		i++;
	}
	i = 0;
}
result = All.FindByType(typeof(TypeRef)).GetByAncs(solution);