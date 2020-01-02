CxList fd = All.FindByType(typeof(FieldDecl)) + All.FindByType(typeof(ConstantDecl));
CxList md = All.FindByType(typeof(MethodDecl));
CxList methods = Find_Methods();

//Custom: find aura inputs
CxList auraInputs = Find_Aura_Inputs();
// Find web services inputs AND global inputs
CxList apexInputs = All.GetParameters(md.FindByFieldAttributes(Modifiers.WebService)) + 
	fd.FindByFieldAttributes(Modifiers.WebService) + 
	All.GetParameters(md.FindByFieldAttributes(Modifiers.Global)) + 
	fd.FindByFieldAttributes(Modifiers.Global) +
	auraInputs;

// Find references inputs
apexInputs.Add(All.FindByMemberAccess("pagereference.getparameters").GetMembersOfTarget().FindByShortName("get") + 
	All.FindByMemberAccess("pagereference.geturl") + 				
	All.FindByMemberAccess("currentpage.getparameters").GetMembersOfTarget().FindByShortName("get") + 
	All.FindByMemberAccess("currentPage.geturl"));

CxList potentialInputs = methods.FindByShortName("pagereference")
	+ methods.FindByShortName("currentpagereference")
	+ methods.FindByShortName("currentpage")
	+ methods.FindByShortName("getcurrentpage");
potentialInputs = potentialInputs.GetMembersOfTarget();

apexInputs.Add(potentialInputs.FindByShortName("headers") + 
	potentialInputs.FindByShortName("parameters").GetMembersOfTarget().FindByShortName("get") +
	potentialInputs.FindByShortName("url"));

apexInputs.Add(potentialInputs.FindByShortName("getparameters").GetMembersOfTarget().FindByShortName("get") + 
	potentialInputs.FindByShortName("geturl"));

//CxList potentialC = Find_Apex_Files().FindByType(typeof(UnknownReference)) +
//	Find_Apex_Files().GetMembersOfTarget();
//potentialC -= Find_Apex_Files().FindByAssignmentSide(CxList.AssignmentSide.Left);
//apexInputs.Add(potentialC.FindByShortName("*__c", false));

CxList getparameters = All.FindByMemberAccess("currentpage.getparameters");
apexInputs.Add(getparameters - All.GetTargetOfMembers());

//Find VF inputs - referecnces
potentialInputs = All.FindByShortName("$pagereference")
	+ All.FindByShortName("$currentpagereference")
	+ All.FindByShortName("$currentpage");
potentialInputs = potentialInputs.GetMembersOfTarget();

CxList VF_Inputs = potentialInputs.FindByShortName("headers") + 
	potentialInputs.FindByShortName("parameters").GetMembersOfTarget() + 
	potentialInputs.FindByShortName("url") + 
	All.FindByMemberAccess("$request.*");


// VF inputs - $$i
potentialInputs = Find_VF_I();
CxList relevantVFData = Find_VF_Pages();
relevantVFData = 
	relevantVFData.FindByType(typeof(MethodInvokeExpr))
	+ relevantVFData.FindByType(typeof(UnknownReference));
relevantVFData -= relevantVFData.FindByShortName("cxprotection_*");
relevantVFData -= relevantVFData.FindByName("cx_*");
relevantVFData -= relevantVFData.FindByShortName("$*");
relevantVFData -= relevantVFData.FindByShortName("onclick__*");
relevantVFData -= relevantVFData.FindByShortName("onkeydown__*");
relevantVFData -= relevantVFData.FindByShortName("onselect__*");
CxList input = relevantVFData * relevantVFData.DataInfluencingOn(potentialInputs);

input -= input.DataInfluencingOn(input);
VF_Inputs.Add(input);
CxList decl = All.FindDefinition(input) * md;
VF_Inputs.Add(All.GetParameters(decl));

CxList VFref = Find_Apex_Files().FindAllReferences(All.GetParameters(decl.FindByShortName("set*")));
VFref = All.GetByAncs(VFref.GetFathers().FindByType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
VFref -= VFref.GetTargetOfMembers();

CxList VFref1 = All.FindAllReferences(VFref) - VFref;
//VFref1 -= VFref1.DataInfluencedBy(apexInputs + VFref1);
VFref.Add(VFref1);
//VFref = All.FindAllReferences(VFref);

CxList ret = Find_Apex_Files().FindByType(typeof(ReturnStmt)).GetByAncs(md.FindByShortName("get*"));
ret = VFref.GetByAncs(ret);
ret -= ret.GetTargetOfMembers();
VF_Inputs.Add(ret);
CxList assign = All.GetByAncs(All.FindByType(typeof(AssignExpr)));
VFref -= assign.GetByAncs(assign.FindByAssignmentSide(CxList.AssignmentSide.Left));
CxList exceptions = VFref.GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr));
exceptions = exceptions.FindByShortName("add", false) + 
	exceptions.FindByShortName("clear", false) + 
	exceptions.FindByShortName("remove", false);
VFref -= exceptions.GetTargetOfMembers();
VF_Inputs.Add(VFref);

VF_Inputs -= VF_Inputs.FindByAssignmentSide(CxList.AssignmentSide.Left);

// Sum up all inputs
result = apexInputs + VF_Inputs;
result -= Find_Id();
result -= Not_Interactive_Inputs();

result -= Find_Test_Code();