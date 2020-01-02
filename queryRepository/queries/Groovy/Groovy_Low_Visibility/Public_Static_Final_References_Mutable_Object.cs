// Get all constant (i.e. "final") objects
CxList constants = All.FindByType(typeof(ConstantDecl));

// Add to "final" also "public" and "static"
CxList publicFinalStatic = constants.
	FindByFieldAttributes(Modifiers.Public).
	FindByFieldAttributes(Modifiers.Static);


// Find all the references of the public final static
CxList pfsRef = All.FindAllReferences(publicFinalStatic);

// Leave only the ones under a return statement
pfsRef = pfsRef.GetByAncs(All.FindByType(typeof(ReturnStmt)));
//remove unknown reference to getter of static final
CxList toRemove = All.NewCxList();
foreach(CxList potentialRef in pfsRef)
{
	string myName = potentialRef.GetName();
	myName = string.Concat("get", myName);	
	CxList encapsulatingMethod = potentialRef.GetAncOfType(typeof(MethodDecl)); 	
	CxList getter = encapsulatingMethod.FindByShortName(myName);
	try{
		CSharpGraph pr = potentialRef.data.GetByIndex(0) as CSharpGraph;
		int prLine = pr.LinePragma.Line;
		foreach(CxList gett in getter)
		{
			CSharpGraph g = gett.data.GetByIndex(0) as CSharpGraph;
			int getterMethodLine = g.LinePragma.Line;
			if(getterMethodLine == prLine)
			{
				toRemove.Add(potentialRef);
			}
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
	
}

pfsRef -= toRemove;

// Remove members (such as date.getTime(), which are OK with us
pfsRef -= pfsRef.GetMembersOfTarget().GetTargetOfMembers();

// Now make sure we leave only the ones that are directly under the return value (no manipulation)
CxList pfsInReturn = pfsRef.GetByAncs(pfsRef.GetFathers().FindByType(typeof(ReturnStmt)));

// ...and return the result
result = pfsInReturn;