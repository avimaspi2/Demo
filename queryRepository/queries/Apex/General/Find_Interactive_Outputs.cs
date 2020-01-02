CxList methods = Find_Methods();
CxList webMethods = methods.FindByFieldAttributes(Modifiers.WebService); 
CxList fd = All.FindByType(typeof(FieldDecl)) + All.FindByType(typeof(ConstantDecl));

result = fd.FindByFieldAttributes(Modifiers.WebService)
	+ Find_VF_O()
	+ Find_VF_I(); // When getting input, we also write to screen so it's output

CxList potentialInputs = methods.FindByShortName("pagereference")
	+ methods.FindByShortName("currentpagereference")
	+ methods.FindByShortName("currentpage")
	+ methods.FindByShortName("getcurrentpage");
potentialInputs = potentialInputs.GetMembersOfTarget();

result.Add(potentialInputs.FindByShortName("setheaders") + 
	potentialInputs.FindByShortName("setparameters") + 
	potentialInputs.FindByShortName("setredirect") + 
	potentialInputs.FindByShortName("parameters").GetMembersOfTarget().FindByShortName("get") + 
	potentialInputs.FindByShortName("seturl"));

result -= Find_Id();

result -= Find_Test_Code();