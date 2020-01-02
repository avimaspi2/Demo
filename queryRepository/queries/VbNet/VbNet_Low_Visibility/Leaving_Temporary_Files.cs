CxList tmpFiles = All.FindByName("Path.GetTempFileName", false);
tmpFiles = tmpFiles.DataInfluencingOn(Find_IO());
CxList delete = All.FindByName("File.Delete", false);

foreach(CxList curTmpFile in tmpFiles)
{
	if(curTmpFile.DataInfluencingOn(delete).Count == 0)
	{
		result.Add(curTmpFile);
	}
}