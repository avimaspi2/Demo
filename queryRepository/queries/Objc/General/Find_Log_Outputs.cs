CxList methods = Find_Methods();

// All but NSLog are Log macros used in different places and frameworks.
CxList log = methods.FindByShortNames(new List<string>{
		"ALog","DDLog","DLog","NSLog",
		"NSLogv",	// Since OS X v10.10
		"TFLog","TTLog","ULog",
		"DDLog:", "DLog:", "NSLog:", "TFLog:", "TTLog:", "ULog:" // Swift
});

// Add SAP's MafLogonLog - first parameter is only the log severity
CxList mafLogonLog = methods.FindByShortName("MafLogonLog");
log.Add(All.GetParameters(mafLogonLog) - All.GetParameters(mafLogonLog, 0));

//Add all logs inheriting from NSLog
CxList inheritedLogs = methods.InheritsFrom("NSLog");

result = log;
result.Add(inheritedLogs);