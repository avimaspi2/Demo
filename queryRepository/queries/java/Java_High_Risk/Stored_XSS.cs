CxList db = Find_DB_Out();

CxList readNonDB = Find_Read_NonDB();
//System.getProperties should not be considered as input
CxList listSystemGetPropertiesInInputs = readNonDB.FindByMemberAccess("System.getProperty");
listSystemGetPropertiesInInputs.Add(readNonDB.FindByMemberAccess("System.getProperties"));
readNonDB -= listSystemGetPropertiesInInputs;


CxList read = All.NewCxList();
read.Add(readNonDB);
read.Add(Find_FileSystem_Read());

CxList inputs = All.NewCxList();
inputs.Add(db);
inputs.Add(read);

CxList outputs = Find_XSS_Outputs();
CxList sanitize =  Find_XSS_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);