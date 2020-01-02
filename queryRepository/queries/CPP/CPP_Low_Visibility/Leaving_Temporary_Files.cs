CxList tmpFiles = Insecure_Temporary_File();
tmpFiles.Add(All.GetByAncs(tmpFiles));
tmpFiles = tmpFiles.DataInfluencingOn(Find_IO());
CxList methods = Find_Methods();
CxList delete = methods.FindByShortName("remove")
	+ methods.FindByShortName("unlink");

foreach(CxList curTmpFile in tmpFiles)
{
	if(curTmpFile.DataInfluencingOn(delete).Count == 0)
	{
		result.Add(curTmpFile);
	}
}