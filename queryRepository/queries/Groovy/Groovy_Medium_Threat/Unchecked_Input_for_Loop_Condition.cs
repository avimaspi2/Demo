CxList inputs = Find_Interactive_Inputs();

CxList loops = All.FindByType(typeof(IterationStmt));
CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList conditions = Find_Conditions();
CxList loopConditions = conditions.FindByFathers(loops);
CxList ifConditions = conditions.FindByFathers(ifStmt);

inputs -= inputs.GetByAncs(loopConditions);

CxList unknown = All.FindByType(typeof(UnknownReference));
loopConditions = unknown.GetByAncs(loopConditions);

CxList rIf = loopConditions.GetAncOfType(typeof(IfStmt));
CxList rCond = ifConditions.FindByFathers(rIf);
rCond = All.GetByAncs(rCond);
CxList reff = rCond.FindAllReferences(loopConditions);
CxList loopsReff = loopConditions.FindAllReferences(reff);

loopConditions -= loopsReff.GetByAncs(rIf);

// remove cases like (currentLine != null) which is a type of loop checking
CxList nullLiteral = All.GetByAncs(conditions).FindByType(typeof(NullLiteral));
loopConditions -= All.GetByAncs(nullLiteral.GetAncOfType(typeof(BinaryExpr)));

CxList sanitize = 
	All.FindByMemberAccess("*.index*", false) + 
	All.FindByMemberAccess("*.length", false) + 
	All.FindByMemberAccess("*.count", false);
sanitize.Add(sanitize.GetTargetOfMembers());

sanitize.Add(Find_DB_In());
sanitize.Add(Find_Dead_Code_Contents());

result = loopConditions.InfluencedByAndNotSanitized(inputs, sanitize);