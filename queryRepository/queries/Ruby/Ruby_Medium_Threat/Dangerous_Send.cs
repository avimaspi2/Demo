//System call could cause command injection through send method.
CxList inputs = Find_Interactive_Inputs();
CxList sysCall = All.FindByType(typeof(UnknownReference)).FindByShortName("syscall");
CxList methods = Find_Methods();
CxList send = methods.FindByShortName("send");
CxList sanitize = Find_Integers();
result = send.InfluencedByAndNotSanitized(inputs + sysCall,sanitize);