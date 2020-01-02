//	Environment Injection
//  ---------------------
//  Find all setenv and getenv affected from stored data
///////////////////////////////////////////////////////////////////////

CxList methods = Find_Methods();
CxList relevantMethods = Find_Environment_Inputs() + Find_Environment_Outputs();
	// methods.FindByShortName("setenv") + methods.FindByShortName("getenv");
/*
CxList getenv = methods.FindByShortName("getenv")
	+ methods.FindByShortName("getenv_s")
	+ methods.FindByShortName("_wgetenv")
	+ methods.FindByShortName("_wgetenv_s");
*/
CxList inputs = Find_Interactive_Inputs();

result = relevantMethods.DataInfluencedBy(inputs);