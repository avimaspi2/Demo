// Find all indices of an array, that are influenced by an input with no validation
CxList indices = Find_Indices();
CxList inputs = Find_Inputs();
CxList unknownRefs = Find_UnknownReference();
CxList variables = unknownRefs;
variables.Add(Find_Declarators());

// We only want indices used for assignments
CxList assignedIndexerRefs = Find_IndexerRefs().FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList assignedIndices = indices.GetByAncs(assignedIndexerRefs);

// The sanitization is if there is some if-statements that checks a reference of the index
CxList conditions = Find_Conditions() - Find_TernaryExpr();
CxList conditionVars = unknownRefs.GetByAncs(conditions);
CxList references = variables.FindAllReferences(assignedIndices);
CxList sanitize = references.DataInfluencingOn(conditionVars);

// Result
result = inputs.InfluencingOnAndNotSanitized(assignedIndices, sanitize);