// $ASP

CxList tmpFiles = Find_Member_With_Target("Scripting.FileSystemObject", "GetTempName");
tmpFiles = tmpFiles.DataInfluencingOn(Find_IO());

CxList delete = Find_Member_With_Target_level2("Scripting.FileSystemObject", "GetFile", "Delete");

foreach(CxList curTmpFile in tmpFiles)
{
	if(curTmpFile.DataInfluencingOn(delete).Count == 0)
	{
		result.Add(curTmpFile);
	}
}