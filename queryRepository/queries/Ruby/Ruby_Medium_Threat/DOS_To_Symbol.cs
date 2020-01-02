CxList memAcc = All.FindByType(typeof(MemberAccess));
CxList toSym = memAcc.FindByShortName("to_sym") + memAcc.FindByShortName("intern");
toSym = toSym.GetTargetOfMembers();

CxList inputs = Find_Interactive_Inputs();


result = toSym.DataInfluencedBy(inputs);