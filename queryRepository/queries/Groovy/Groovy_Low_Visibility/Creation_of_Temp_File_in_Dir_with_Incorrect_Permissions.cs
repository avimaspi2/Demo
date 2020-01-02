CxList createFile = 
	All.FindByMemberAccess("File.createNewFile") +
	All.FindByMemberAccess("File.createTempFile");

CxList newFileObject = All.FindByShortName("File").GetAncOfType(typeof(ObjectCreateExpr));
CxList permissions = All.FindByMemberAccess("File.setExecutable")
	+ All.FindByMemberAccess("File.setReadable")
	+ All.FindByMemberAccess("File.setWritable");

CxList insecureCreate = createFile - createFile.DataInfluencedBy(permissions);
CxList createInfluenced = newFileObject.DataInfluencingOn(insecureCreate);

result = createInfluenced - createInfluenced.DataInfluencingOn(createInfluenced);