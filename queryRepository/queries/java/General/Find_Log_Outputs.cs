CxList write = All.NewCxList();
write.Add(All.FindByName("*Debug.Write", false));
write.Add(All.FindByName("*Log.Write", false));
write.Add(All.FindByName("*Debug.WriteLine", false));
write.Add(All.FindByName("*Log.WriteLine", false)); 
write.Add(All.FindByName("*Log.debug", false)); 
write.Add(All.FindByName("*Log.error", false));
write.Add(All.FindByMemberAccess("ServletContext.log"));
write.Add(All.FindByMemberAccess("GenericServlet.log"));
write.Add(All.FindByMemberAccess("LogWriter.write"));
write.Add(All.FindByMemberAccess("Category.assertLog"));//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.debug"));	//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.error"));	//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.fatal")); 	//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.forcedLog"));//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.info"));		//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.l7dlog")); 	//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.log"));		//org.apache.log4j.Category
write.Add(All.FindByMemberAccess("Category.warn"));		//org.apache.log4j.Category

CxList logMembers = All.FindByMemberAccess("Log.*").GetTargetOfMembers();

CxList log = logMembers.FindByType("Log");
log.Add(logMembers.FindByShortName("Log", false));

write.Add(log.GetMembersOfTarget());
log = All.FindByMemberAccess("Logger.*").GetTargetOfMembers();
/* Look for types that inherit from Logger */
CxList allInheritsFromLogger = All.InheritsFrom("Logger");


List<string> types = new List<string>();
foreach(CxList type in allInheritsFromLogger) {
	CSharpGraph cType = type.TryGetCSharpGraph<CSharpGraph>();
	if(cType.ShortName != null && cType.ShortName.Length > 0) {
		types.Add(cType.ShortName);
	}
}
CxList allLog = log.FindByType("Logger");
allLog.Add(log.FindByShortName("Logger", false));
allLog.Add(log.FindByTypes(types.ToArray()));

write.Add(allLog.GetMembersOfTarget());
write.Add(All.FindByMemberAccess("EventLogger.logEvent"));
write.Add(All.FindByMemberAccesses(new string[]{"AbstractLogger.catching*","AbstractLogger.checkMessageFactory","AbstractLogger.debug","AbstractLogger.ent*", 
	"AbstractLogger.error","AbstractLogger.exit","AbstractLogger.fatal","AbstractLogger.info","AbstractLogger.log*",
	"AbstractLogger.printf","AbstractLogger.throwing*","AbstractLogger.trace*","AbstractLogger.warn"}, true));
                                                
write.Add(All.FindByMemberAccesses(new string[]{"ExtendedLoggerWrapper.catching*","ExtendedLoggerWrapper.checkMessageFactory","ExtendedLoggerWrapper.debug",
	"ExtendedLoggerWrapper.ent*","ExtendedLoggerWrapper.error","ExtendedLoggerWrapper.exit","ExtendedLoggerWrapper.fatal",
	"ExtendedLoggerWrapper.info","ExtendedLoggerWrapper.log*","ExtendedLoggerWrapper.printf","ExtendedLoggerWrapper.throwing*",
	"ExtendedLoggerWrapper.trace*","ExtendedLoggerWrapper.warn"}, true));

CxList notWrite = All.NewCxList();
notWrite.Add(write.FindByMemberAccesses(new string[]{"Logger.getLogger","Logger.getMyLogger","Logger.getJADELogger","Logger.is*","Logger.set*"}, true));
notWrite.Add(write.FindByMemberAccess("Log.is*"));

result = /*log + */ write - notWrite;
result -= Find_Dead_Code_Contents();