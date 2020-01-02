// One must not use "loadLibrary", but should use "System.Load" to be more secure
CxList methods = Find_Methods();
CxList loadLibrary = methods.FindByMemberAccess("System.loadLibrary");

result = loadLibrary;