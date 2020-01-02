CxList createFile = All.FindByMemberAccess("File.createNewFile");
createFile.Add(All.FindByMemberAccess("File.createTempFile"));

CxList newFileObject = All.FindByShortName("File").GetAncOfType(typeof(ObjectCreateExpr));

CxList permissions = All.FindByMemberAccess("File.setExecutable");
permissions.Add(All.FindByMemberAccess("File.setReadable"));
permissions.Add(All.FindByMemberAccess("File.setWritable"));

CxList insecureCreate = createFile - createFile.DataInfluencedBy(permissions);
CxList createInfluenced = newFileObject.DataInfluencingOn(insecureCreate);

result = createInfluenced - createInfluenced.DataInfluencingOn(createInfluenced);