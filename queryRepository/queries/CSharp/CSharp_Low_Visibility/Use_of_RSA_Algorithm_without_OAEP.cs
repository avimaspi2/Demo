CxList rsa = All.FindByMemberAccess("RSACryptoServiceProvider.Encrypt");
CxList rsaParam = All.GetParameters(rsa, 1);
CxList inputs = Find_Inputs();
CxList negative = inputs + All.FindByType(typeof(BooleanLiteral)).FindByShortName("false");

result = rsaParam * negative + rsaParam.DataInfluencedBy(negative);