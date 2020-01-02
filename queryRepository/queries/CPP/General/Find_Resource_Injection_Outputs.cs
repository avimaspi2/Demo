CxList methods = Find_Methods();
// The second parameter of the connect function contains the address of the server to which the socket will connect to
CxList socketConnect = All.GetParameters(methods.FindByShortName("connect"));
result.Add(socketConnect);

// add environment variables queries to resources
CxList getEnvVariable = methods.FindByShortName("GetEnvironmentVariable");
CxList getEnvVariableParams = All.GetParameters(getEnvVariable, 0);
result.Add(getEnvVariableParams);

// add registry queries to resources
CxList regQueryValue = methods.FindByShortName("RegQueryValueEx"); //param 1
CxList regQueryParams = All.GetParameters(regQueryValue, 1);
result.Add(regQueryParams);

CxList regGetValue = methods.FindByShortName("RegGetValue"); //params 1,2
CxList regGetParams = All.GetParameters(regGetValue, 1);
regGetParams.Add(All.GetParameters(regGetValue, 2));
result.Add(regGetParams);

// Add file openings to resources
CxList files = All.GetParameters(All.FindByTypes(new string[]{"ifstream","fstream"}),0);
CxList openFilesMethods = Find_Open_Files_Methods();

// Exclude open files methods with read-only access mode
CxList fileAccessMode = All.GetParameters(openFilesMethods, 1);
openFilesMethods -= openFilesMethods.FindByParameters(fileAccessMode.FindByShortName("r"));

files.Add(All.GetParameters(openFilesMethods, 0));

result.Add(files);