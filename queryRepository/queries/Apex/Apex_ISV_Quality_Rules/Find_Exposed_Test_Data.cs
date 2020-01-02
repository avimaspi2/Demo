//Find test cases that are dependent on org data - using either @isTest(SeeAllData=true) or 
//@isTest with a version older than 23.0

//Find isTest(SeeAllData=true)
CxList isTest = All.FindByCustomAttribute("istest");
CxList isTestChildren = All.GetByAncs(isTest);
CxList boolTest = isTestChildren.FindByType(typeof(BooleanLiteral)).FindByShortName("true");
boolTest = boolTest.GetFathers();
CxList seeAllData = isTestChildren.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByShortName("seealldata");
result = seeAllData.FindByFathers(boolTest).GetAncOfType(typeof(CustomAttribute));

//Find isTest on an old version
string oldVersion = "23.0";
isTest -= result;
CxList apiVersions = All.FindByMemberAccess("apiversion.text");
apiVersions = Find_Strings().GetByAncs(apiVersions.GetFathers()).FindByAssignmentSide(CxList.AssignmentSide.Right);
if (apiVersions.Count > 0)
{
	foreach(CxList curTest in isTest) 
	{
		try 
		{	//Find the versions in the xml matching curTest's filename        
			LinePragma lp = ((CSharpGraph) curTest.data.GetByIndex(0)).LinePragma;
			if (lp != null && lp.FileName != null)
			{
				string filename = lp.FileName;
				CxList curVersion = apiVersions.FindByFileName(filename + "*");
				if (curVersion.Count > 0)
				{
					string versionString = ((CSharpGraph) curVersion.data.GetByIndex(0)).ShortName;
					if (versionString != null)
					{
						versionString = versionString.Trim('"', '\'');
						if (versionString.IndexOf(".") == 1) //Check for a very old version "X.--"
						{
							versionString = "0" + versionString;
						}
						if (String.Compare(versionString, oldVersion) <= 0)
						{
							result.Add(curTest.Concatenate(curVersion, true));
						}
					}
				}
			}
		}
		catch (Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	}
}