CxList unknownRefs = Find_UnknownReference();
CxList whiteListSanitizers = All.NewCxList();

whiteListSanitizers.Add(Find_WhiteListSanitizers_CustomMethods());
whiteListSanitizers.Add(Find_WhiteListSanitizers_SwitchStatements());
whiteListSanitizers.Add(Find_WhiteListSanitizers_SwitchStatementsWithReturn());
whiteListSanitizers.Add(Find_WhiteListSanitizers_IfStatements());

CxList allReferences = All.NewCxList();

foreach(CxList sanitizer in whiteListSanitizers)
{
	CxList currentMethod = sanitizer.GetAncOfType(typeof(StatementCollection));
	allReferences.Add(unknownRefs.FindAllReferences(whiteListSanitizers).GetByAncs(currentMethod));
}
		
CxList referenceDefinitions = All.FindDefinition(allReferences);

CxList referenceDefinitionsInFields = referenceDefinitions.GetByAncs(Find_FieldDecls());

CxList inFieldReferences = allReferences.FindAllReferences(referenceDefinitionsInFields);

CxList toRemove = All.NewCxList();
toRemove.Add(inFieldReferences);
toRemove.Add(allReferences.FindByShortName("this"));

result = allReferences - toRemove;