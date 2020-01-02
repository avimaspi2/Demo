//Give warning if api version is 2 releases old. 

//NOTE: The current version and amount of old versions are hardcoded in the query.
//current version is: 26.0
double currentVersion = 26.0;
//the amount of versions back to warn is: 2
double n = 2;
string oldVersion = (currentVersion - n).ToString();

CxList apiVersions = All.FindByMemberAccess("apiversion.text");
apiVersions = Find_Strings().GetByAncs(apiVersions.GetFathers()).FindByAssignmentSide(CxList.AssignmentSide.Right);
foreach(CxList curVersion in apiVersions) 
{
	try 
	{	        
		string versionString = ((CSharpGraph) curVersion.data.GetByIndex(0)).ShortName;
		if (versionString != null)
		{
			versionString = versionString.Trim('"', '\'');
			if (versionString.IndexOf(".") == 1) //Check for a very old version "X.--"
			{
				versionString = "0" + versionString;
			}
			if (String.Compare(versionString, oldVersion) < 0)
			{
				result.Add(curVersion);
			}
		}
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}