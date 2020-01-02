//CxList methods = All.FindByType(typeof(MethodDecl));
//// Try in 2 different ways
//CxList throws = All.FindByRegex(@"\)(\s)+throws(\s)+(Exception|Throwable)");
//throws = methods.GetMethod(throws);
///
//// Removing results above, since we get too many double results
///

CxList paramDeclCollection = All.FindByType(typeof(ParamDeclCollection));
CxList exception = paramDeclCollection.FindByRegex(@"throws\sException");
CxList throwable = paramDeclCollection.FindByRegex(@"throws\sThrowable");

result = /*throws + */exception + throwable;