//	Multi-Byte String Length
//  ------------------------
//  Find Incorrect Calculation of Multi-Byte String Length
///////////////////////////////////////////////////////////////////////

// Find all method calls
CxList methods = Find_Methods();

// Find all mallocs
CxList mallocMethods = methods.FindByShortName("malloc");
CxList mallocParams = methods.GetParameters(mallocMethods);

// 1. strlen of Multi-Byte String is wrong
// Find the strlen fun	ctions that are direct parameters of malloc
CxList lengthMethods = mallocParams.FindByShortName("strlen") + mallocParams.FindByShortName("lstrlen");
CxList mbStringLength = All.GetParameters(lengthMethods).FindByType("long");

// 2. malloc with wcslen parameter (i.e. without multiply expr) is wrong
CxList wcslenUnderMalloc = mallocParams.FindByShortName("wcslen");

result = mbStringLength + wcslenUnderMalloc;