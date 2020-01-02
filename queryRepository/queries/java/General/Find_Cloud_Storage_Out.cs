/**
*	This query will gather storage outputs from cloud services
*/

// Get All AmazonAWS imports
CxList amazonawsImports = All.FindByName("com.amazonaws.*").FindByType(typeof(Import));


// Search for all Files with those imports:
System.Collections.ArrayList fileNamesWithImports = new System.Collections.ArrayList();
foreach (CxList import in amazonawsImports)
{
	if (import.data.Count > 0)
	{
		CSharpGraph g = import.TryGetCSharpGraph<CSharpGraph>(); 
		if (g != null)
		{
			string fileName = g.LinePragma.FileName;
			if (!fileNamesWithImports.Contains(fileName))
			{
				fileNamesWithImports.Add(fileName);
			}
		}
	}
}

// Find All TransferManager usages 
CxList allMethods = Find_Methods();
CxList awsS3outs = All.NewCxList();
awsS3outs.Add(allMethods.FindByMemberAccess("TransferManager", "upload"));
awsS3outs.Add(allMethods.FindByMemberAccess("AmazonS3.deleteObject"));
awsS3outs.Add(allMethods.FindByMemberAccess("AmazonS3.deleteObjects"));
awsS3outs.Add(allMethods.FindByMemberAccess("AmazonS3.putObject"));
awsS3outs.Add(allMethods.FindByMemberAccess("AmazonS3.initiateMultipartUpload"));

// return all AWS TransferManager:
foreach (CxList upload in awsS3outs)
{
	if (upload.data.Count > 0)
	{
		CSharpGraph g = upload.TryGetCSharpGraph<CSharpGraph>(); 
		if (g != null)
		{
			string fileName = g.LinePragma.FileName;
			if (fileNamesWithImports.Contains(fileName))
			{
				result.Add(upload);
			}
		}
	}
}