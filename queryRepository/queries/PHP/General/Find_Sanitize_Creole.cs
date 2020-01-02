CxList methods = Find_Methods();

// 1 - Find creole prepared statements to search for 2 cases of sanitizing 
CxList creoleConnections = methods.FindByMemberAccess("Creole.getConnection", false);
List<string> preparedStatementNames = new List<string>{"prepareStatement", "prepareCall"};
CxList preparedStatements = methods.FindByShortNames(preparedStatementNames, false);
CxList creolePreparedStatements = preparedStatements.DataInfluencedBy(creoleConnections);

// 2 - Find associated sanitizers
// 2.1 - Case 1: Invoking bind methods on created statements is a way of sanitizing
List<string> bindMethodNames = new List<string>{"set", "setString", "setArray"};
CxList bindMethods = methods.FindByShortNames(bindMethodNames, false);
CxList creolePreparedStatementBindMethods = bindMethods.DataInfluencedBy(creolePreparedStatements);
	
// 2.2 - Case 2: Sending an argument to executeUpdate/executeQuery on created statements is also a way of sanitizing
List<string> executionMethodNames = new List<string>{"executeQuery", "executeUpdate"};
CxList executionMethods = methods.FindByShortNames(executionMethodNames, false);
CxList creolePreparedStatementExecutionMethods = executionMethods.DataInfluencedBy(creolePreparedStatements);
CxList creolePreparedStatementExecBoundArgs = All.GetParameters(creolePreparedStatementExecutionMethods, 0);

// 3 - Return found cases
result.Add(creolePreparedStatementBindMethods);
result.Add(creolePreparedStatementExecBoundArgs);