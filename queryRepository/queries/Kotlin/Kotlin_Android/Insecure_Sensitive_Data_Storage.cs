CxList sensitiveData = Find_Sensitive_Data();
CxList androidStorage = Find_Android_Storage();
result = androidStorage.DataInfluencedBy(sensitiveData);