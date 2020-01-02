CxList methods = Find_Methods();

// 1 - Find connection references
CxList creoleConnections = methods.FindByMemberAccess("Creole.getConnection", false);
    
// 2 - Find implicit database access points (in)
// 2.1 - Find creole preparecall/preparestatement invokes (may receive sql as param)
List<string> preparedStatementNames = new List<string>{"prepareStatement", "prepareCall"};
CxList preparedStatements = methods.FindByShortNames(preparedStatementNames, false);
CxList creolePreparedStatements = preparedStatements.DataInfluencedBy(creoleConnections);
CxList creolePreparedStatementNodes = creolePreparedStatements.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

// 2.2 - Find param bindings on creole prepared statements
List<string> preparedSetterNames = new List<string>{"set", "setArray", "setString"};
CxList preparedSetters = methods.FindByShortNames(preparedSetterNames, false);
CxList creolePreparedSetters = preparedSetters.DataInfluencedBy(creolePreparedStatements);
CxList creolePreparedSetterNodes = creolePreparedSetters.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
    
// 2.3 - Find args of execution methods on from creole connections (execution can occur on connection or prepared statement)
List<string> executionMethodNames = new List<string>{"executeQuery", "executeUpdate", "execute"};
CxList executionMethods = methods.FindByShortNames(executionMethodNames, false);
CxList creoleExecutionMethods = executionMethods.DataInfluencedBy(creoleConnections);
CxList creoleExecutionMethodNodes = creoleExecutionMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList creoleExecutionMethodArgs = All.GetParameters(creoleExecutionMethodNodes, 0);

// 3 - Return found points
result.Add(creolePreparedStatementNodes);
result.Add(creolePreparedSetterNodes);
result.Add(creoleExecutionMethodArgs);