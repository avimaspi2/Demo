/// <summary>
///  This query serches for session secret that is stored without encryption.
/// </summary>
///
CxList fieldDecls = Find_FieldDecls();
CxList associativeArrayExprs = Find_AssociativeArrayExpr();
CxList secrets = Find_Personal_Info();

fieldDecls -= fieldDecls.GetByAncs(associativeArrayExprs);

secrets -= secrets.FindByType(typeof(Declarator));
secrets -= fieldDecls;
secrets -= secrets.FindByType(typeof(ParamDecl));

CxList methods = Find_Methods();
CxList session = methods.FindByShortName("session") - XS_Find_All();
session.Add(Find_UnknownReference().FindByShortName("session", false));

CxList encrypt = Find_Encrypt();

CxList SourcesList = All.NewCxList();
CxList assignRight = All.FindByAssignmentSide(CxList.AssignmentSide.Right);

// for each secret - check if there is a session parameter assigned a secret without encryption.
foreach(CxList secret in secrets)
{
	if (secret.FindByAssignmentSide(CxList.AssignmentSide.Left).Count > 0)
	{
		SourcesList.Add(assignRight.FindByFathers(secret.GetFathers())); // the value assigned to a secret parameter
	}																	// {secret: value}
	else if (secret.TryGetCSharpGraph<FieldDecl>() is FieldDecl field &&
		field.Declarators.Count > 0)
	{
			var expr = field.Declarators[0].InitExpression;
			SourcesList.Add(expr.NodeId, expr);
	}
	else
	{
		SourcesList.Add(secret);	// find secret assigned to a session parameter {parameter: secret}
	}								// {parameter: secret} , {parameter: secret + token} ...
}

// Remove assigning of empty strings from secrets
CxList emptyString = Find_String_Literal().FindByShortName("");
SourcesList -= emptyString;
SourcesList -= Find_NullLiteral();

result = SourcesList.InfluencingOnAndNotSanitized(session, encrypt);
result.Add(SourcesList.InfluencingOnAndNotSanitized(NodeJS_Find_DB_IN(), encrypt));

// remove duplicate results where 'secret' and 'secret + token' are in sources
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);