CxList objCFiles = Find_ObjC_Write();

CxList logOutputs = Find_Log_Outputs();
CxList writeC = Find_Write_C();

result.Add(objCFiles);
result.Add(logOutputs);
result.Add(writeC);