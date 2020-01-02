CxList appendParameter = 
	All.FindByMemberAccess("parameters.append") +
	All.FindByMemberAccess("parameters.add") +
	All.FindByMemberAccess("parameters.addwithvalue");

CxList parameters_ = All.FindByShortName("Parameters_*").GetMembersOfTarget();

result = 
	All.GetParameters(appendParameter) + 
	parameters_.FindByShortName("value") +
	All.FindByType("sqlparameter") +
	All.FindByType("oracleparameter");