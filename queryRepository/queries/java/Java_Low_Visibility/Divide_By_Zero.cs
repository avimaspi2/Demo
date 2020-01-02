// Direct division by zero

// dead code
CxList deadCode = Find_Dead_Code_Contents();

// Find all zeros. They are inserted to suspiction list 
// because each zero may be part of x/0 calculation
CxList zero = All.FindByName("0").FindByType(typeof(IntegerLiteral));	// Random (2)
zero -= deadCode;

CxList bin = Find_BinaryExpr();

// "call" to any array index, like : a[1]
CxList indexer = Find_IndexerRefs();

// The "UnknownReference" type returns the any type of variables, not methods call fro example, or
// any refference to array, but variables
CxList unknown = Find_UnknownReference();
unknown -= deadCode;

IAbstractValue zeroAbsValue = new IntegerIntervalAbstractValue(0);
unknown = unknown.FindByAbstractValue(abstractValue => zeroAbsValue.IncludedIn(abstractValue, true));

// We can be apply method by following way
//			for(int i = 0; i < layer_def->nChildren() ; i++) {
//					SGPropertyNode * acloud = layer_def->getChild(i);
// Note, that i has value 0, but method that applied is getChiled(i)
// so this cases should be removed as well, in order to prevent false positive cases
// we need to take care very carifully. 
// One one hand if we will remove ALL methods from suspiction list, we can remove right cases like
//		int f(int p){
//			return 0;
//		}
//So just methods that there deffinition is not declared will be removed from the list

// find all methods
CxList methods = Find_Methods();

// find methods defenitins
CxList metDefinitions = Find_MethodDeclaration();//All.FindDefinition(methods);
// remove from the methods list that will be sanitisers, methods calls that have definitions
// so just methods appling without difinition is stay in sanitize list
// reason: we do not know if this method realy returs zero, so in order to avoid false positive
// we assume that it not returns zero
methods -= methods.FindAllReferences(metDefinitions);

CxList cond = unknown * All.GetByAncs(Find_Conditions());

CxList sanitize = All.NewCxList();
sanitize.Add(bin);
sanitize.Add(indexer);
sanitize.Add(methods);

// the potential divide by zero is path from zero to variable that not passed throw sanitize plus zero list
// In general, potentialDiv can include the expression like 0/5, because 0 in the zero list
// To resolve it, the IF statement below will take just right side in consadiration
//CxList potentialDiv = unknown * unknown.InfluencedByAndNotSanitized(zero, sanitize) + zero;
CxList divBin = All.NewCxList();
foreach (CxList b in bin)
{
	try
	{
		BinaryExpr g = b.TryGetCSharpGraph<BinaryExpr>();
		if (g != null && 
			(g.Operator == BinaryOperator.Divide || g.Operator == BinaryOperator.Modulus))
		{		
			divBin.Add(g.NodeId, g);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList unknownIndiv = unknown.GetByAncs(divBin);
CxList potentialDiv = unknownIndiv * unknownIndiv.InfluencedByAndNotSanitized(zero, sanitize);
potentialDiv.Add(zero);

potentialDiv = potentialDiv.FindByFathers(divBin);

// The potential Bin is object of the DOM type "Binary Expression"
CxList potentialBin = potentialDiv.GetFathers();
CxList conditions = cond.FindAllReferences(potentialDiv);

CxList divWithoutIf = All.NewCxList();	// All the divs that are not in an "if" statement
foreach (CxList b in potentialBin)
{
	try
	{
		BinaryExpr g = b.TryGetCSharpGraph<BinaryExpr>();
		if (g != null && 
			(g.Operator == BinaryOperator.Divide || g.Operator == BinaryOperator.Modulus))
		{		
			Expression right = g.Right;
			if (right != null)
			{
				// We are under BinaryOperator.Divide/Modulus if statement , just x/y expression took in considiration
				// where x and y are Expressions type in DOM. In case the binary expression will be in format
				// x/(a+b) the right side will be a+b. and nodeId of "+" will be returned. In any case 
				// potential variable below will point to the right side expression NodeId
				CxList potential = potentialDiv.FindById(right.NodeId);
					
				//the if below hanlde x/0 cases
				if (potential.FindByName("0").Count == 1)
				{
					result.Add(right.NodeId, right);
				}
				else
				{
					int len = right.Text.IndexOf(" Inst#");
					if (len >= 0)
					{
						string text = right.Text.Substring(0, len);							
						text = text.Substring(text.LastIndexOf(".") + 1);
						CxList d = potentialDiv.FindByFathers(b);
						if (d.FindByShortName(text).Count == 1)
						{
							CxList ifStmtCheck = conditions.FindAllReferences(potential).GetAncOfType(typeof(IfStmt));
							ifStmtCheck.Add(conditions.FindAllReferences(potential).GetAncOfType(typeof(TernaryExpr)));
							ifStmtCheck = potential.GetByAncs(ifStmtCheck);
							if (ifStmtCheck.Count == 0)
							{			
								divWithoutIf.Add(right.NodeId, right);
							}
						}
					} 
				}
			}
		}
		
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
result.Add(zero.InfluencingOnAndNotSanitized(divWithoutIf, sanitize));


// Division by input or Random
CxList inputs = Find_Inputs(); // input
inputs.Add(All.FindByMemberAccess("Random.next*"));// Random (1)
inputs.Add(All.FindByMemberAccess("Math.random"));
inputs.Add(Get_ESAPI().FindByMemberAccess("Randomizer.*")); // ESAPI


inputs -= deadCode;

potentialBin = All.NewCxList();
foreach (CxList b in bin)
{
	try
	{
		BinaryExpr g = b.TryGetCSharpGraph<BinaryExpr>();
		if (g != null && 
			(g.Operator == BinaryOperator.Divide || g.Operator == BinaryOperator.Modulus))
		{
			potentialBin.Add(g.NodeId, g);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

potentialDiv = unknown * unknown.GetByAncs(potentialBin).DataInfluencedBy(inputs);
conditions = cond.FindAllReferences(potentialDiv);

divWithoutIf = All.NewCxList();
foreach (CxList b in potentialBin)
{
	try
	{
		BinaryExpr g = b.TryGetCSharpGraph<BinaryExpr>();
		if (g != null && g.Right != null)
		{
			CxList gRight = All.FindById(g.Right.NodeId);
			CxList div = potentialDiv.GetByAncs(gRight);
			CxList ifStmtCheck = conditions.FindAllReferences(div).GetAncOfType(typeof(IfStmt));
			foreach (CxList d in div)
			{
				CxList specificIfStmtCheck = d.GetByAncs(ifStmtCheck);
				if (specificIfStmtCheck.Count == 0)
				{
					CSharpGraph dg = d.TryGetCSharpGraph<CSharpGraph>();
					divWithoutIf.Add(dg.NodeId, dg);
				}
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
result.Add(inputs.DataInfluencingOn(divWithoutIf));

result -= deadCode;