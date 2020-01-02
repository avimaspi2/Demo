CxList imports = Find_Imports();

String[] logsMethods = new string[] {"debug", "error", "exception", "warning", "info", "critical"  };

result = Find_Methods_By_Import("logging", logsMethods, imports);

String[] warningsMethods = new string[] {"warn", "warn_explicit", "warnpy3k", "showwarning"};
result.Add(Find_Methods_By_Import("warnings", warningsMethods, imports));

CxList getLogger = All.InfluencedBy(
	Find_Methods_By_Import("logging", new string[]{"getLogger"}, imports))
	.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList getLoggerMemberAccess = All.FindAllReferences(
	getLogger).GetMembersOfTarget();

foreach(String s in logsMethods){
	result.Add(getLoggerMemberAccess.FindByShortName(s));
}

result.Add( Find_HTTPServer_Log() );