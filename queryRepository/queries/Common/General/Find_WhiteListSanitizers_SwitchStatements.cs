// Auxiliar Lists
CxList methodDecls = Find_MethodDecls();
CxList parameters = Find_Param();
CxList validMethods = All.NewCxList();
CxList expressions = Find_Expressions();
CxList returnStmts = Find_ReturnStmt();
CxList iterationStmts = Find_IterationStmt();
CxList unknowRefs = Find_UnknownReference();
CxList switchStmts = Find_SwitchStmt();
CxList cases = All.FindByType(typeof(Case));
CxList paramDecls = Find_ParamDecl();
CxList AllMethodStatements = Find_StatementCollection();
CxList methods = Find_Methods();

//Checks an variable is being validated in a switch
// Example: 
//switch hostX {
//	case "example.com":
//		fallthrough
//	case "example.org":
//		fallthrough
//	case "example.net":
//		conn, _ := net.Dial("tcp", hostX)
//		fmt.Fprintf(conn, text + "\n")
//	default:
//		return
//			}
//}
foreach (CxList methodDecl in methodDecls)
{
	CxList methodStatements = AllMethodStatements.FindByFathers(methodDecl);
	CxList thisMethodSwitches = switchStmts.FindByFathers(methodStatements);
		
	foreach(CxList switchStmt in thisMethodSwitches)
	{
		CxList switchCases = cases.FindByFathers(switchStmt);
	
		CxList primitiveExpressions = All.FindByFathers(switchCases).FindByAbstractValue(x => !(x is AnyAbstractValue));
		bool hasDefault = false;
	
		foreach(CxList switchCase in switchCases)
		{
			Case caseObj = switchCase.TryGetCSharpGraph<Case>();
		
			if(caseObj.IsDefault)
			{
				hasDefault = true;
				break;
			}
		}
	
		if(hasDefault && primitiveExpressions.Count == switchCases.Count - 1)
		{
			CxList switchCondition = expressions.FindByFathers(switchStmt);
			CxList methodParms = paramDecls.FindDefinition(switchCondition);
			
			if(methodParms.Count == 0)
			{
				continue;
			}
			
			int index = methodParms.GetIndexOfParameter();
			CxList thisMethodInvocations = methods.FindAllReferences(methodDecl);
			
			if(thisMethodInvocations.Count > 0)
			{
				validMethods.Add(All.GetParameters(thisMethodInvocations, index) - parameters);
				result.Add(validMethods);
			}
			else
			{
				result.Add(methodParms);
			}
		}
	}

}