// Commands list
CxList commands = Find_Command_Injection();
CxList system = commands.FindByShortName("system");
CxList exec = commands.FindByShortName("exec");
CxList open = commands.FindByShortName("open");

// Sanitizers
CxList sanitize = All.GetByAncs(All.GetParameters(commands));
CxList notSanitizer = sanitize.GetByAncs(All.GetParameters(system, 0));
notSanitizer.Add(sanitize.GetByAncs(All.GetParameters(exec, 0)));

// 2nd parameter of open is a sanitizer
CxList openParam2 = sanitize.GetByAncs(All.GetParameters(open, 1));
openParam2 -= openParam2.GetByAncs(openParam2.FindByShortName("<*").GetAncOfType(typeof(Param))); 
notSanitizer.Add(openParam2);
sanitize -= notSanitizer;

// Integers are also sanitizers
sanitize.Add(Find_Integers());

result = sanitize;