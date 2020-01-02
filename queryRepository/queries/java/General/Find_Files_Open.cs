CxList objCreate = Find_Object_Create();
CxList methods = Find_Methods();

CxList fileOpen = objCreate.FindByShortName("File");

/*
	According to their documentations, the following constructors can also access/alter Files.
*/

fileOpen.Add(objCreate.FindByShortNames(new List<string> {"FileDataSource", "FileWriter", "FileReader", "FileInputStream", "FileOutputStream"}));

/*
	Sometimes there's no flow between the object creator and its parameters.
	Hence, adding the left side of the assign expressions of fileOpeningStatements
	to the possible end nodes, as well as their declarators.
*/
fileOpen.Add(fileOpen.GetAssignee(All));

/*
	The following methods access/create files as well, according to their documentations.
*/
CxList filesMethods = methods.FindByMemberAccess("Files.*");
fileOpen.Add(filesMethods.FindByShortNames(new List<string>{
		"createFile","createTempFile","newBufferedReader","newBufferedWriter","newByteChannel",
		"newInputStream","newOutputStream","readAllBytes","readAllLines","write"}));
fileOpen.Add(methods.FindByMemberAccess("File.createTempFile"));

result = fileOpen;