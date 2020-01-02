/*Find insufficient output length for scrypt hash*/

CxList scryptBadKeyLenAttribution = Find_Not_In_Range("golang.org/x/crypto/scrypt", "Key", 5, 32, null);



/*Find insufficient output length for pbkdf2 hash*/

CxList pbkdf2BadKeyLenAttribution = Find_Not_In_Range("golang.org/x/crypto/pbkdf2", "Key", 3, 32, null);



/*Find insufficient output length for argon2 hash*/

//find the argon2 objects of lhecker and tvdburgt libs
List<string> argon2Methods = new List<string>{"Config", "DefaultConfig"};
List<string> goArgon2Methods = new List<string>{"Context", "NewContext"};
CxList argon2Configuration = All.FindByMemberAccess("\"github.com/lhecker/argon2\".*").FindByShortNames(argon2Methods);
argon2Configuration.Add(All.FindByMemberAccess("\"github.com/tvdburgt/go-argon2\".*").FindByShortNames(goArgon2Methods));
//find the argon2 references occurences
CxList argon2ConfigReferences = All.FindAllReferences(argon2Configuration.GetAncOfType(typeof(Declarator)));
//find the argon2.HashLength references occurences
CxList argon2HashLengthReferences = argon2ConfigReferences.GetMembersOfTarget().FindByShortName("HashLen*");
//find the attributions to argon2.HashLength
CxList argon2HashLengthAttributionVariables = argon2HashLengthReferences.GetAssigner();
//find the declarations where it is defined a good hash output length ( >= 32)
CxList absIntegers = All.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);

IAbstractValue absValue = new IntegerIntervalAbstractValue(32, long.MaxValue);
CxList range = absIntegers.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absValue));

//find the attributions to argon2.HashLength with a short hash output length ( < 31) by excluding the good hash output lengths
CxList argon2GoodHashLengthAttribution = argon2HashLengthAttributionVariables.FindByAbstractValues(range);
CxList argon2BadHashLengthAttribution = argon2HashLengthAttributionVariables - argon2GoodHashLengthAttribution;

//find insufficient output length for argon2 hash of magical and pzduniak libs
argon2BadHashLengthAttribution.Add(Find_Not_In_Range("github.com/magical/argon2", "Key", 5, 32, null));
argon2BadHashLengthAttribution.Add(Find_Not_In_Range("github.com/pzduniak/argon2", "Key", 5, 32, null));



/*Add the occurences results*/
result.Add(scryptBadKeyLenAttribution.ReduceFlowByPragma());
result.Add(pbkdf2BadKeyLenAttribution.ReduceFlowByPragma());
result.Add(argon2BadHashLengthAttribution.ReduceFlowByPragma());