CxList methods = Find_Methods();

CxList otherCommandsWithException = methods.FindByMemberAccess("System.loadLibrary");
CxList propGetPropMethod = methods.FindByMemberAccess("Properties.getProperty");

result = Find_IO() + Find_DB() + otherCommandsWithException;
result -= propGetPropMethod;
result -= result.GetParameters(result); // leave only commands, not parameters
result -= result.GetByAncs(methods.FindByShortName("getAttribute")); // remove "2nd-level" inputs and DB