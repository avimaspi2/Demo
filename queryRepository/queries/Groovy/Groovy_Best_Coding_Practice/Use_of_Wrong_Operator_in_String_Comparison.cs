CxList AllBinaryExpr = All.FindByType(typeof(BinaryExpr));
CxList cmp = AllBinaryExpr.GetByBinaryOperator(BinaryOperator.IdentityEquality) + 
	AllBinaryExpr.GetByBinaryOperator(BinaryOperator.IdentityInequality);

CxList MethodInv = Find_Methods();
CxList AllMemberAccess = All.FindByType(typeof(MemberAccess));

CxList NULL = All.FindByName("null").FindByFathers(cmp).GetFathers();
CxList InvTargets = MethodInv.GetTargetOfMembers().FindByType("String"); 
CxList AllMemberAccessTargets = AllMemberAccess.GetTargetOfMembers().FindByType("String"); 

CxList stringLiteral = All.FindByType(typeof(StringLiteral));
CxList allStrings = All.FindByType("String") + All.FindAllReferences(All.FindByReturnType("String")) + stringLiteral;
CxList stringsCompared = allStrings.FindByFathers(cmp - NULL) - InvTargets - AllMemberAccessTargets;

CxList binaryComparers = stringsCompared.GetAncOfType(typeof(BinaryExpr));

int count = 0;
while (binaryComparers.Count > 0 && count++ < 10)
{
	result.Add(binaryComparers * cmp);
	binaryComparers = binaryComparers.GetFathers().GetAncOfType(typeof(BinaryExpr));
}