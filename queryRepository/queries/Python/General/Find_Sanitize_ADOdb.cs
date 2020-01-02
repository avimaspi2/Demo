CxList methods = Find_DB_In_ADOdb();
CxList allParams = Find_Param();

CxList execute = methods.FindByName("*.Execute");
//only consider the second parameter of execute as a sanitizer
result.Add(allParams.GetParameters(execute, 1));

//UpdateBlob(self,table,field,blob,where,blobtype='BLOB')
CxList updateBlob = methods.FindByName("*.UpdateBlob");
CxList updateBlobFile = methods.FindByName("*.UpdateBlobFile");
//only the parameter 'blob' is sanitized
result.Add(allParams.GetParameters(updateBlob, 2));
result.Add(allParams.GetParameters(updateBlobFile, 2));