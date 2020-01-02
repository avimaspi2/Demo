/*
 * This query finds unsecure passwords working like this:
 *  - First finds all possible passwords
 *  - Then remove all passwords that are correctly encrypted by strong encrypters
 *  - Remove also passwords that are result from those strong encrypters
 *  - Finally find where the passwords are used alerting which ones will be stored.
*/ 

CxList sensitiveData = Find_Passwords();
CxList methods = Find_Methods();

CxList cryptoImports = All.NewCxList();

cryptoImports.Add(Find_Weak_Encryptors().GetFathers());
cryptoImports.Add(All.FindAllReferences(cryptoImports));

CxList relevantWriteMethods = cryptoImports.GetMembersOfTarget().FindByShortNames(new List<string>(){"Write"});
relevantWriteMethods = relevantWriteMethods.DataInfluencedBy(sensitiveData).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
relevantWriteMethods -=  All.GetAssigner();
CxList sensitiveCryptoVariables = relevantWriteMethods.GetTargetOfMembers();


CxList writeOutputs = All.NewCxList();
writeOutputs.Add(Find_DB_In());
writeOutputs.Add(Find_Write());
writeOutputs.Add(Find_Log_Outputs());

CxList strongEncrypters = All.NewCxList();
// Accept pbkdf2 encription:
strongEncrypters.Add(All.FindByMemberAccess("\"golang.org/x/crypto/pbkdf2\".Key"));
// Accept bcrypt encription:
strongEncrypters.Add(All.FindByMemberAccess("\"golang.org/x/crypto/bcrypt\".GenerateFromPassword"));
// Accept scrypt encription:
strongEncrypters.Add(All.FindByMemberAccess("\"golang.org/x/crypto/scrypt\".Key"));
// Accept argon2 encription (github.com/tvdburgt/go-argon2 implementation):
List<string> argon2Methods = new List<string>(){"HashEncoded","Hash"};
strongEncrypters.Add(All.FindByMemberAccess("\"github.com/tvdburgt/go-argon2\".*").FindByShortNames(argon2Methods));
// Accept argon2 encription (github.com/lhecker/argon2 implementation):
CxList argonCfg = All.FindByMemberAccess("\"github.com/lhecker/argon2\".DefaultConfig");
CxList argonCfgOcurrences = All.FindAllReferences(argonCfg.GetAssignee());
List<string> argon2methods = new List<string>(){"HashEncoded","HashRaw","Hash"};
strongEncrypters.Add(argonCfgOcurrences.GetMembersOfTarget().FindByShortNames(argon2methods));


// the following line is for functions with more than one return values
CxList encryptedPasswords = strongEncrypters.GetFathers().GetAssignee();
encryptedPasswords.Add(strongEncrypters.GetAssignee());
encryptedPasswords.Add(All.FindAllReferences(encryptedPasswords));
sensitiveData -= encryptedPasswords;
sensitiveData.Add(sensitiveCryptoVariables);

result = writeOutputs
	.InfluencedByAndNotSanitized(sensitiveData, strongEncrypters)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);