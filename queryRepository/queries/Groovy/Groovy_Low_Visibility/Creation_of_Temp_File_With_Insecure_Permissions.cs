CxList createNewFile = All.FindByMemberAccess("File.createTempFile");

CxList writeToFile = Find_Methods().FindByShortName("write*");
CxList permissions = 
	All.FindByMemberAccess("File.setExecutable")
	+ All.FindByMemberAccess("File.setReadable")
	+ All.FindByMemberAccess("File.setWritable");

CxList insecureWrite = writeToFile - writeToFile.DataInfluencedBy(permissions);
result = createNewFile.DataInfluencingOn(insecureWrite);