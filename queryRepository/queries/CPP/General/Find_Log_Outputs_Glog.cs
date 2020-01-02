// https://github.com/google/glog

List<string> names = new List<string> {
	"DLOG",
	"DLOG_EVERY_N",
	"DLOG_IF",
	"DLOG_IF_EVERY_N",
		
	"LOG",
	"LOG_EVERY_N",
	"LOG_FIRST_N",
	"LOG_IF",
	"LOG_IF_EVERY_N",
				
	"VLOG",
	"VLOG_EVERY_N",
	"VLOG_IF",
	"VLOG_IF_EVERY_N",
	};

CxList methods = Find_Methods();
CxList logMethods = methods.FindByShortNames(names);
CxList logMethodsVars = logMethods.GetAssignee();
CxList logMethodsInputs = methods.GetByAncs(logMethodsVars.GetAssigner());
result = logMethodsInputs;