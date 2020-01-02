CxList inputs = Find_Inputs_NoWindowLocation();
inputs.Add(All.FindAllReferences(inputs.GetAssignee()));

CxList fields = Find_Declarators();
CxList trueAbstractValued = All.FindByAbstractValue(absVal => absVal is TrueAbstractValue);
trueAbstractValued.Add(All.FindByType(typeof(BooleanLiteral)).FindByShortName("true"));
CxList methods = Find_Methods();

CxList unsafeNoentOptions = fields.FindByShortName("noent") * trueAbstractValued.GetAssignee();
CxList unsafelibXmljsMethods = methods.FindByMemberAccess("*.parseXml").DataInfluencedBy(unsafeNoentOptions);
unsafelibXmljsMethods = unsafelibXmljsMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
result = inputs.DataInfluencingOn(unsafelibXmljsMethods);