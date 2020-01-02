CxList write = All.NewCxList();
CxList notWrite = All.NewCxList();
CxList methodInvokeExprs = Find_Methods();

write.Add(methodInvokeExprs.FindByName("*Debug.Write", false));
write.Add(methodInvokeExprs.FindByName("*Debug.WriteLine", false));
write.Add(methodInvokeExprs.FindByName("*Log.Write", false));
write.Add(methodInvokeExprs.FindByName("*Log.WriteLine", false));
write.Add(methodInvokeExprs.FindByName("*Log.debug", false)); 
write.Add(methodInvokeExprs.FindByName("*Log.error", false));
write.Add(methodInvokeExprs.FindByName("*Log.info", false));
write.Add(methodInvokeExprs.FindByName("*Log.trace", false));
write.Add(methodInvokeExprs.FindByName("*Log.warn", false));
write.Add(methodInvokeExprs.FindByMemberAccess("ServletContext.log"));
write.Add(methodInvokeExprs.FindByMemberAccess("GenericServlet.log"));
write.Add(methodInvokeExprs.FindByMemberAccess("LogWriter.write"));

CxList log = All.FindByMemberAccess("Log.*").GetTargetOfMembers();
log = log.FindByType("Log") + log.FindByShortName("Log", false);
write.Add(log.GetMembersOfTarget());

log = All.FindByMemberAccess("Logger.*").GetTargetOfMembers();
log = log.FindByType("Logger") + log.FindByShortName("Logger", false);
write.Add(log.GetMembersOfTarget());

notWrite.Add(write.FindByMemberAccess("Logger.getLogger"));
notWrite.Add(write.FindByMemberAccess("Logger.is*"));
notWrite.Add(write.FindByMemberAccess("Logger.set*"));
notWrite.Add(write.FindByMemberAccess("Log.is*"));

result = write - notWrite;
result -= Find_Dead_Code_Contents();