// http://www.boost.org/doc/libs/1_66_0/libs/log/doc/html/index.html

List<string> names = new List<string> {
		"BOOST_LOG",
		"BOOST_LOG_SEV",
		"BOOST_LOG_TRIVIAL"
		};

CxList methods = Find_Methods();
CxList logMethods = methods.FindByShortNames(names);
CxList logMethodsVars = logMethods.GetAssignee();
CxList logMethodsInputs = methods.GetByAncs(logMethodsVars.GetAssigner());
result = logMethodsInputs;