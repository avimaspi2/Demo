CxList AllBinaryExpr = Find_BinaryExpr();

CxList cmp = AllBinaryExpr.GetByBinaryOperator(BinaryOperator.IdentityEquality); 
cmp.Add(AllBinaryExpr.GetByBinaryOperator(BinaryOperator.IdentityInequality));

CxList MethodInv = Find_Methods();
CxList AllMemberAccess = Find_MemberAccesses();

CxList NULL = All.FindByName("null").FindByFathers(cmp).GetFathers();
CxList InvTargets = MethodInv.GetTargetOfMembers().FindByType("String"); 
CxList AllMemberAccessTargets = AllMemberAccess.GetTargetOfMembers().FindByType("String"); 


CxList stringLiteral = Find_String_Literal();
CxList allStrings = All.FindByType("String");
allStrings.Add(All.FindAllReferences(All.FindByReturnType("String")));
allStrings.Add(stringLiteral);

CxList stringsCompared = allStrings.FindByFathers(cmp - NULL) - InvTargets - AllMemberAccessTargets;
stringsCompared -= stringsCompared.GetMembersOfTarget().GetTargetOfMembers();

CxList binaryComparers = stringsCompared.GetAncOfType(typeof(BinaryExpr));;

int count = 0;
while (binaryComparers.Count > 0 && count++ < 10)
{
	result.Add(binaryComparers * cmp);
	binaryComparers = binaryComparers.GetFathers().GetAncOfType(typeof(BinaryExpr));
}