CxList methods = Find_Methods();
methods = methods.GetTargetOfMembers().GetMembersOfTarget();

result.Add(All.FindByShortName("*WriteErrorLogtoExcel*"));
result.Add(methods.FindByMemberAccess("*CommonLoggerEngine.CommonErrorLog*", false));

//Adding log4Net member access
result.Add(methods.FindByMemberAccess("ILog.Debug*", false));
result.Add(methods.FindByMemberAccess("ILog.DebugFormat*", false));
result.Add(methods.FindByMemberAccess("ILog.ErrorFormat*", false));
result.Add(methods.FindByMemberAccess("ILog.Error*", false));
result.Add(methods.FindByMemberAccess("ILog.Fatal*", false));
result.Add(methods.FindByMemberAccess("ILog.FatalFormat*", false));
result.Add(methods.FindByMemberAccess("ILog.Info*", false));
result.Add(methods.FindByMemberAccess("ILog.InfoFormat*", false));
result.Add(methods.FindByMemberAccess("ILog.Warn*", false));
result.Add(methods.FindByMemberAccess("ILog.WarnFormat*", false));

result.Add(methods.FindByMemberAccess("*Debug.Write*", false));
result.Add(methods.FindByMemberAccess("*Trace.Write*", false));
result.Add(methods.FindByMemberAccess("*Log.Write*", false));
CxList write = methods.FindByShortName("Write*", false);
result.Add(write.GetTargetOfMembers().FindByShortName("*Log", false).GetMembersOfTarget());
result.Add(methods.FindByMemberAccess("LogWriter.Write")); // EndLib
result.Add(methods.FindByMemberAccess("*Logger.Write", false));    // EntLib

result.Add(methods.FindByMemberAccess("Logger.Trace")); // NLog
result.Add(methods.FindByMemberAccess("Logger.Debug")); // NLog
result.Add(methods.FindByMemberAccess("Logger.Info"));  // NLog
result.Add(methods.FindByMemberAccess("Logger.Warn"));  // NLog
result.Add(methods.FindByMemberAccess("Logger.Error")); // NLog
result.Add(methods.FindByMemberAccess("Logger.Fatal")); // NLog

result.Add(methods.FindByMemberAccess("ILogger.Log"));
result.Add(methods.FindByMemberAccess("ILogger.LogCritical"));
result.Add(methods.FindByMemberAccess("ILogger.LogError"));
result.Add(methods.FindByMemberAccess("ILogger.LogWarning"));
result.Add(methods.FindByMemberAccess("ILogger.LogInformation"));
result.Add(methods.FindByMemberAccess("ILogger.LogDebug"));
result.Add(methods.FindByMemberAccess("ILogger.LogTrace"));
	
result.Add(methods.FindByMemberAccess("Trace.Trace*")); // C# Trace
result.Add(methods.FindByMemberAccess("Trace.Write*")); // C# Trace

result.Add(methods.FindByMemberAccess("EventLog.WriteEntry")); // Event log
result.Add(methods.FindByMemberAccess("EventLog.WriteEvent")); // Event log