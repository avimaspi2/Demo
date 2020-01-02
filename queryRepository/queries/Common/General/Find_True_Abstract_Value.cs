result = All.FindByAbstractValue(abstractValue => abstractValue is TrueAbstractValue);

// Adding constants references
CxList constants = Find_Constants();
CxList trueInConstants = All.GetByAncs(constants).FindByShortName("true");
CxList trueConstantDecls = trueInConstants.GetAncOfType(typeof(FieldDecl));
CxList trueConstantRefs = All.FindAllReferences(trueConstantDecls);
result.Add(trueConstantRefs);