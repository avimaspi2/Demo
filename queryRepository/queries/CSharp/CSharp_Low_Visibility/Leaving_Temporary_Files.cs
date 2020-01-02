CxList tmpFiles = All.FindByName("Path.GetTempFileName");
tmpFiles = tmpFiles.DataInfluencingOn(Find_IO());
CxList delete = All.FindByName("File.Delete");

foreach(CxList curTmpFile in tmpFiles)
{
	if(curTmpFile.DataInfluencingOn(delete).Count == 0)
	{
		result.Add(curTmpFile);
	}
}