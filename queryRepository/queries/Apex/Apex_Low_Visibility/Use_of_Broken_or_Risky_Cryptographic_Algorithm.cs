CxList methods = All.FindByType(typeof(MethodInvokeExpr));
CxList digest = methods.FindByMemberAccess("crypto.generatedigest") + methods.FindByMemberAccess("crypto.generatemac");
CxList cryptoFunc = All.FindByType(typeof(StringLiteral));

cryptoFunc = cryptoFunc.FindByShortNames(new List<string> {"\'sha1\'", "\'md5\'", "\"sha1\"", "\"md5\"" });

CxList sanitize = digest.GetTargetOfMembers(); // Eliminate flows throught the target of the digest methods

result.Add(digest.InfluencedByAndNotSanitized(cryptoFunc, sanitize));