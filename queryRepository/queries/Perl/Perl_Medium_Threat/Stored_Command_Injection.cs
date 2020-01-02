CxList methods = Find_Methods();

// TODO: add backticks
CxList exec = methods.FindByShortName("exec");
CxList syscall = methods.FindByShortName("syscall");
CxList system = methods.FindByShortName("system");
CxList open = methods.FindByShortName("open");
CxList qx = methods.FindByShortName("qx");
CxList readpipe = methods.FindByShortName("readpipe");

CxList commands = exec + syscall + system + open + qx + readpipe;
CxList inputs = Find_DB_Out() + Find_Read();
CxList sanitize = All.GetByAncs(All.GetParameters(commands));
sanitize -= sanitize.GetByAncs(All.GetParameters(system, 0));
sanitize -= sanitize.GetByAncs(All.GetParameters(exec, 0));
CxList openParam2 = sanitize.GetByAncs(All.GetParameters(open, 1));
openParam2 -= openParam2.GetByAncs(openParam2.FindByShortName("<*").GetAncOfType(typeof(Param))); 
// 2nd parameter of open is a sanitizer
sanitize -= openParam2;
sanitize.Add(Find_Integers());

result = commands.InfluencedByAndNotSanitized(inputs, sanitize);