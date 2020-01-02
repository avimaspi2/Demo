CxList methods = Find_Methods();

CxList myBatisParam = All.FindByShortName("mybatisParam");

myBatisParam.Add(myBatisParam.GetMembersOfTarget());

CxList binary = All.FindByType(typeof(BinaryExpr));

CxList notSantized = myBatisParam.GetByAncs(binary);

CxList methodNotSanitized = myBatisParam.FindDefinition(notSantized).GetFathers().GetFathers();

CxList sanitized = methods.FindByParameters(myBatisParam).GetFathers().GetFathers().GetFathers();

sanitized -= methodNotSanitized;

sanitized.Add(myBatisParam.GetParameters(sanitized));

result = All.FindAllReferences(sanitized);