//////////////////////////////////////////////////////
// Query Suspected_XSS
//
// This query finds suspected XSS
// Every session.getAttribute is defined as suspected XSS input
// We try find targets for every non active output (without flow)
//////////////////////////////////////////////////////

// Every session attribute is suspected as input
CxList getAttribs = Find_Jsp_Code().FindByMemberAccess("session.getAttribute");

CxList assignGetAttribs = getAttribs.GetAncOfType(typeof(AssignExpr)); 
CxList DeclStmtGetAttribs = getAttribs.GetAncOfType(typeof(VariableDeclStmt));

CxList inputs = All.GetByAncs(assignGetAttribs + DeclStmtGetAttribs).FindByAssignmentSide(CxList.AssignmentSide.Left);

// Refine sanitizer
CxList methodReturnInt = All.FindByReturnType("int");
CxList returnStatments = Find_ReturnStmt();
CxList methodInvSanitizers = returnStatments.GetByMethod(methodReturnInt);

CxList sanitized = Find_XSS_Sanitize();
sanitized.Add(Find_DB_In());
sanitized.Add(methodInvSanitizers);
// End refine sanitizer

CxList outputs = Find_XSS_Outputs();

// Refine outputs
// Get non active nodes - Output nodes with no flow from input
CxList activeNodes = inputs.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList nonActiveNodes = (outputs - activeNodes).FindByType(typeof(MethodInvokeExpr));

// Try to find flow to the targets of the non-active nodes
CxList unknownRefs = nonActiveNodes.GetTargetOfMembers();
unknownRefs = unknownRefs - methodInvSanitizers.GetTargetOfMembers();

outputs.Add(unknownRefs);

CxList binExprOutputs = outputs.FindByType(typeof(BinaryExpr));
CxList membersInOutputs = All.GetByAncs(binExprOutputs).FindByType(typeof(MethodInvokeExpr));

outputs -= binExprOutputs;
outputs.Add(membersInOutputs);
// end Refine outputs

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);