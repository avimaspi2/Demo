CxList unknownRefs = Find_UnknownReference();
CxList redisRequire = Find_Require("redis");

CxList client = redisRequire.GetMembersOfTarget().FindByShortName("createClient").GetAssignee();
CxList clientRefs = unknownRefs.FindAllReferences(client).GetMembersOfTarget();

List<string> methods =  new List<string>{"del",	"expire", "hset", "hmset", "mset", "rpush",	"sadd",	"set"};

result = clientRefs.FindByShortNames(methods, true);