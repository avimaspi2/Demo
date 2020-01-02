// Find the string literals containig "password"
CxList psw = Find_Password_Strings();

// Find creation of connections or connection strings, influenced by password
CxList createExpressions = Find_Object_Create();
CxList openConnection = createExpressions.FindByShortName("*Connection");

result = openConnection.DataInfluencedBy(psw);

// Find password in relevant DB connection class initialization. There are three cases: 
// 1. DataSource.getConnection(String user, String password)
CxList dataSourceConn = All.FindByMemberAccess("DataSource.getConnection");
// 2. DriverManager.getConenction(String url, String user, String password)
CxList driverManagerConn = All.FindByMemberAccess("DriverManager.getConnection");
// 3. DriverManagerDataSource.setPassword(String password)
CxList dmdsSetPassword = All.FindByMemberAccess("DriverManagerDataSource.setPassword");
// 4. new SimpleDriverDataSource(Driver instance, String url, String user, String password)
CxList simpleDriverDataSource = createExpressions.FindByShortName("SimpleDriverDataSource");

CxList pswInitMethods = All.NewCxList();
pswInitMethods.Add(dataSourceConn);
pswInitMethods.Add(driverManagerConn);
pswInitMethods.Add(dmdsSetPassword);
pswInitMethods.Add(simpleDriverDataSource);

CxList pswParamInMethod = All.GetParameters(dataSourceConn, 1);
pswParamInMethod.Add(All.GetParameters(driverManagerConn, 2));
pswParamInMethod.Add(All.GetParameters(dmdsSetPassword, 0));
pswParamInMethod.Add(All.GetParameters(simpleDriverDataSource, 3));

CxList pswStrings = pswParamInMethod.FindByType(typeof(StringLiteral));
CxList pswStringParam = pswParamInMethod.FindByType("String");
CxList influencedParam = pswStringParam.DataInfluencedBy(Find_Strings());
pswStrings.Add(influencedParam.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

CxList hardcodedStringSanitizers = All.FindByMemberAccess("ResultSet.*");
List<string> safeMethods = new List<string>{ "getNString", "getString" };

CxList sanitizers = Find_ESAPI_Sanitizer();
sanitizers.Add(hardcodedStringSanitizers.FindByShortNames(safeMethods));

// Add the path from the string/parameter to its method
result.Add(pswInitMethods.InfluencedByAndNotSanitized(pswStrings, sanitizers));