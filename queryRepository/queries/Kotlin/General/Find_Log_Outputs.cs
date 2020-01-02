CxList write = All.NewCxList();
write.Add(All.FindByName("*Debug.Write", false));
write.Add(All.FindByName("*Log.Write", false));
write.Add(All.FindByName("*Debug.WriteLine", false));
write.Add(All.FindByName("*Log.WriteLine", false)); 
write.Add(All.FindByName("*Log.debug", false)); 
write.Add(All.FindByName("*Log.error", false));
write.Add(All.FindByName("*Log.info", false));
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

CxList log = All.FindByMemberAccess("Log.*").GetTargetOfMembers();

CxList logClone = All.NewCxList();
logClone = log.FindByType("Log");
logClone.Add(log.FindByShortName("Log", false));

write.Add(logClone.GetMembersOfTarget());

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

logClone = All.NewCxList();
logClone = log.FindByType("Logger");
logClone.Add(log.FindByShortName("Logger", false));
logClone.Add(log.FindByTypes(types.ToArray()));

write.Add(logClone.GetMembersOfTarget());

CxList notWrite = All.NewCxList();
notWrite.Add(write.FindByMemberAccess("Logger.getLogger"));
notWrite.Add(write.FindByMemberAccess("Logger.getMyLogger"));
notWrite.Add(write.FindByMemberAccess("Logger.getJADELogger"));
notWrite.Add(write.FindByMemberAccess("Logger.is*"));
notWrite.Add(write.FindByMemberAccess("Logger.set*"));
notWrite.Add(write.FindByMemberAccess("Log.is*"));

result = /*log + */ write - notWrite;
//result -= Find_Dead_Code_Contents();