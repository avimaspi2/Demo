CxList requireOfHTTP = Find_Require("http");
requireOfHTTP.Add(Find_Require("https"));
requireOfHTTP.Add(Find_Require("restify"));

////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	Get file names of all files that required db-mysql or mysql DB and all query methods for those DB
*/
List<String> allFilesWithHTTP = new List<String>();
CxList allINHTTP = All.NewCxList();

foreach(CxList reqHttp in requireOfHTTP)
{
	try
	{
		CSharpGraph reqHttpGR = reqHttp.GetFirstGraph();
		String reqHttpName = reqHttpGR.LinePragma.FileName;
		if (!allFilesWithHTTP.Contains(reqHttpName))
		{
			allFilesWithHTTP.Add(reqHttpName);
			allINHTTP.Add(All.FindByFileName(reqHttpName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result = allINHTTP;