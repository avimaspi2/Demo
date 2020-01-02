CxList write = All.NewCxList();

write.Add(All.FindByName("*Log.Write", false));
write.Add(All.FindByName("*Log.WriteLine", false)); 
write.Add(All.FindByName("*Log.debug", false)); 
write.Add(All.FindByName("*Log.error", false));
write.Add(All.FindByName("*Log.fatal", false));
write.Add(All.FindByName("*Log.warn", false));
write.Add(All.FindByName("*Log.info", false));
write.Add(All.FindByName("*Log.trace", false));

write.Add(All.FindByName("*Logger.Write", false));
write.Add(All.FindByName("*Logger.WriteLine", false)); 
write.Add(All.FindByName("*Logger.debug", false)); 
write.Add(All.FindByName("*Logger.error", false));
write.Add(All.FindByName("*Logger.fatal", false));
write.Add(All.FindByName("*Logger.warn", false));
write.Add(All.FindByName("*Logger.info", false));
write.Add(All.FindByName("*Logger.trace", false));

write.Add(All.FindByMemberAccess("ServletContext.log"));
write.Add(All.FindByMemberAccess("GenericServlet.log"));
write.Add(All.FindByMemberAccess("LogWriter.write"));
write.Add(All.FindByShortName("Log", false));

CxList log = All.NewCxList();

log = All.FindByType("Log");
log.Add(All.FindByType("Logger"));
write.Add(log.GetMembersOfTarget());

CxList notWrite = All.NewCxList();

notWrite.Add(write.FindByMemberAccess("Logger.getLogger"));
notWrite.Add(write.FindByMemberAccess("Logger.getLog"));
notWrite.Add(write.FindByMemberAccess("Logger.is*"));
notWrite.Add(write.FindByMemberAccess("Logger.set*"));
notWrite.Add(write.FindByMemberAccess("Log.getLogger"));
notWrite.Add(write.FindByMemberAccess("Log.getLog"));
notWrite.Add(write.FindByMemberAccess("Log.set*"));
notWrite.Add(write.FindByMemberAccess("Log.is*"));

//Referent to akka.event.Logging methods
CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();
// ex:  val l = Logging(context.system, this)
CxList akkaLogging = methods.FindByShortName("Logging");
CxList akkaLoggingAssignee = akkaLogging.GetAssignee();
CxList akkaLoggingMember = akkaLogging.GetMembersOfTarget();
CxList akkaLoggingMemberReferences = unknownRefs.FindAllReferences(akkaLoggingAssignee);

akkaLoggingMember.Add(akkaLoggingMemberReferences.GetMembersOfTarget());
CxList possibleAkkaLoggingMethods = methods.FindByShortNames(new List<string>{"info", "error", "debug", "warning", "log"});
CxList akkaLoggingMethods = akkaLoggingMember * possibleAkkaLoggingMethods;
akkaLoggingMethods.Add(possibleAkkaLoggingMethods.FindByMemberAccess("*LoggingAdapter.*"));

result = write.FindByType(typeof(MethodInvokeExpr)) - notWrite;
result.Add(akkaLoggingMethods);