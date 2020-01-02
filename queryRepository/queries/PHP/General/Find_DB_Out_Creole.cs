CxList methods = Find_Methods();

// 1 - Find connection references
CxList creoleConnections = methods.FindByMemberAccess("Creole.getConnection", false);

// 2 - Find implicit database access points (out)
// 2.1 - Find methods for fetching results from connection or statement
List<string> queryMethodNames = new List<string>{
		"executeQuery", 
		"execute", 
		"getResultSet"
	};
CxList queryMethods = methods.FindByShortNames(queryMethodNames, false);
CxList creoleQueryMethods = queryMethods.DataInfluencedBy(creoleConnections);
CxList creoleQueryMethodsNodes = creoleQueryMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

// 2.2 - Find methods for getting data from executed callable prepared statement
CxList callablePreparedStatements = methods.FindByShortName("prepareCall");
CxList creoleCallablePreparedStatements = callablePreparedStatements.DataInfluencedBy(creoleConnections);
List<string> getterMethodNames = new List<string>{
		"getString", 
		"getArray", 
		"get"
		};
CxList getterMethods = methods.FindByShortNames(getterMethodNames, false);
CxList creoleGetterMethods = getterMethods.DataInfluencedBy(creoleCallablePreparedStatements);
CxList creoleGetterMethodsNodes = creoleGetterMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

// 3 - Return found points
result.Add(creoleQueryMethodsNodes);
result.Add(creoleGetterMethodsNodes);