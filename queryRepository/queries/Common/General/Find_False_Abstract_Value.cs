result = All.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);

// Adding constants references
CxList constants = Find_Constants();
CxList falseInConstants = All.GetByAncs(constants).FindByShortName("false");
CxList falseConstantDecls = falseInConstants.GetAncOfType(typeof(FieldDecl));
CxList falseConstantRefs = All.FindAllReferences(falseConstantDecls);
result.Add(falseConstantRefs);