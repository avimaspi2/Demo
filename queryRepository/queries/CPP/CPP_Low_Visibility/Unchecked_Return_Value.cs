CxList conditions = Get_Conditions();
CxList methodsWithReturnValue = Find_Methods_With_Return_Value();

CxList voidCasts = Find_TypeRef().FindByShortName("void").GetAncOfType(typeof(CastExpr));
 
// remove the ones that are being casted to void
methodsWithReturnValue -= methodsWithReturnValue.FindByFathers(voidCasts);

CxList assignMethods = methodsWithReturnValue.GetAncOfType(typeof(AssignExpr));
assignMethods.Add(methodsWithReturnValue.GetAncOfType(typeof(VariableDeclStmt)));

methodsWithReturnValue -= methodsWithReturnValue.GetByAncs(assignMethods);
methodsWithReturnValue -= conditions;

CxList assignValues = All.GetByAncs(assignMethods).FindByAssignmentSide(CxList.AssignmentSide.Left);
assignValues -= assignValues.FindAllReferences(conditions.FindAllReferences(assignValues));

result = methodsWithReturnValue;
result.Add(assignValues);