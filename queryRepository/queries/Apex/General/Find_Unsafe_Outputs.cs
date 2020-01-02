CxList vf = Find_VF_Pages();

// Find unsafe outptuts
CxList outputs = Find_Interactive_Outputs() * vf;
CxList escapeFalse = vf.FindByType("cx__protoff__o") +
	vf.FindByType("cx__protoff__i");
escapeFalse = outputs * outputs.DataInfluencingOn(escapeFalse);

CxList classOutputs = Find_Interactive_Outputs() * Find_Apex_Files();

CxList unsafeOutputs = classOutputs + escapeFalse +
	outputs.FindByName("*.script") + 
	outputs.FindByShortName("*style") +
	outputs.FindByShortName("style*") +
//	outputs.FindByName("*.src") +
//	outputs.FindByName("*.url") +
	outputs.FindByShortName("on*") +
	outputs.FindByName("*.apex.sectionheader.help") +
	outputs.FindByName("*script.value");

// Ignore id outputs
CxList dollarOutputs = Find_VF_I() + Find_VF_O();

CxList id = Find_Id();
CxList idOutputs = id.GetAncOfType(typeof(AssignExpr)) + 
	id.GetAncOfType(typeof(Declarator));
idOutputs = dollarOutputs.GetByAncs(idOutputs);
CxList componentParams = unsafeOutputs.GetByAncs(vf.FindByType(typeof(AssignExpr)));

CxList filter = All.FindByShortName("$o*") + All.FindByShortName("$i*");
filter = filter.GetMembersOfTarget() +
	idOutputs +
	componentParams
	- 
	outputs.FindByName("*.script");


result = unsafeOutputs - filter;