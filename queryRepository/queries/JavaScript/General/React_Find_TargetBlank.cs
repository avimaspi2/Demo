CxList reactAll = React_Find_All();
CxList associativeArrays = reactAll * Find_AssociativeArrayExpr();
CxList declarators = reactAll * Find_Declarators();

CxList targetBlankFields = All.NewCxList();

CxList targetFields = declarators.GetByAncs(associativeArrays).FindByShortName("target");
foreach (CxList target in targetFields)
{
	CxList targetDecl = target.GetAncOfType(typeof(Declarator));
	Declarator targetDeclObj = targetDecl.TryGetCSharpGraph<Declarator>();
	if (targetDeclObj.InitExpression.Text == "_blank")
	{
		CxList attributesObj = targetDecl.GetAncOfType(typeof(AssociativeArrayExpr));
		CxList relFields = declarators.GetByAncs(attributesObj).FindByShortName("rel");
		if (relFields.Count == 0)
		{
			targetBlankFields.Add(target);
		}
		else //Check if sanitized by rel="noopener" or rel="noopener noreferrer"
		{	
			foreach (CxList rel in relFields)
			{
				CxList relDecl = rel.GetAncOfType(typeof(Declarator));
				Declarator relDeclObj = relDecl.TryGetCSharpGraph<Declarator>();
				if (!Regex.IsMatch(relDeclObj.InitExpression.Text, @"(\s)*noopener((\s+)noreferrer(\s)*)?"))
				{
					targetBlankFields.Add(target);
				}
			}
		}
	}
}

result = targetBlankFields;