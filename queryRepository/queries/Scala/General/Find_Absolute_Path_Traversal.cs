// This query searched for opening file statements which path is influenced by an input.
// By default, the query will return Absolute_Path_Traversal, but the input and sanitizers lists
// 	can be overridden to reflect relative path traversal and stored path traversal.
// param[0] is CxLIst inputs (or Find_Interactive_Inputs() by default)
// param[1] is CxList sanitizers (or Find_Path_Traversal_Sanitize() by default)
//	The rest of the parameters are ignored.
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Path_Traversal_Sanitize();
if (param.Length > 0)
{
	try
	{
		if (param[0] != null)
			inputs = param[0] as CxList;

		if (param.Length > 1)
		{
			if (param[1] != null)
				sanitize = param[1] as CxList;
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex.Message);
	}
}


CxList filesMethods = All.FindByMemberAccess("Files.*") + All.FindByMemberAccess("*.Files.*");
CxList methodParams = All.GetParameters(filesMethods);
methodParams -= methodParams.FindByTypes(new string[]{"string", "InputStream", "OutputStream"});	// if the first or second parameters are of these types - then they are not vulnerable to path traversal
methodParams -= methodParams.FindByType(typeof(StringLiteral));

// in java.nio.Files : these parameters of java.nio.Files methods are vulnerable to path traversal 
CxList vulnerableParams = methodParams.GetParameters(filesMethods, 0); 
string[] methodsSecondParamNames = {"copy", "move", "createLink", "createSymbolicLink"};
CxList filesMethodsSecondParam = filesMethods.FindByShortNames(new List<string>(methodsSecondParamNames));
vulnerableParams.Add(methodParams.GetParameters(filesMethodsSecondParam, 1)); 

// in java.io.File and java.nio.File: the constructor parameters are vulnerable to path traversal
CxList objFile = Find_Object_Create().FindByShortName("File*") + Find_Object_Create().FindByShortName("*.File*");

CxList createTempFile = All.FindByMemberAccess("File.createTempFile") + All.FindByMemberAccess("*.File.createTempFile"); 
vulnerableParams.Add(All.GetParameters(createTempFile, 2)); // the 3rd parameter is a path to create the temp file in

objFile.Add(vulnerableParams);

result = objFile.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);