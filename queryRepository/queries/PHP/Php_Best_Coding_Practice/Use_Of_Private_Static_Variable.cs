//this query looks for explicit private static declaration of variables
//static variable declaration inside a private method is considered static private

//find private static declaration of class members (FieldDecl)
CxList variableDeclStatic = All.FindByType(typeof(FieldDecl))
	.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Static)
	.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Private);

CxList methodDecls = All.FindByType(typeof(MethodDecl));
CxList unknownRefs = All.FindByType(typeof(UnknownReference));
CxList declarators = All.FindByType(typeof(Declarator));

CxList privateMethodDecl = methodDecls.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Private);

//find static variables inside private methods
CxList staticDeclaratorsInPrivateMethod = declarators.GetByAncs(privateMethodDecl).FindByRegex("static");

//Build dicrionary of methodDecl line pragma by fileID
Dictionary<int, HashSet<int>> methodLinePragmaByFiles = new Dictionary<int, HashSet<int>>();
foreach(CxList methodDecl in privateMethodDecl)
{
	CSharpGraph methodDeclCSG = methodDecl.data.GetByIndex(0) as CSharpGraph;

	if(methodDeclCSG != null && methodDeclCSG.LinePragma != null && methodDeclCSG.LinePragma.Line != 0)
	{
		int fileID = methodDeclCSG.LinePragma.GetFileId();
		int methodDeclLine = methodDeclCSG.LinePragma.Line;
		
		if (!methodLinePragmaByFiles.ContainsKey(fileID))
		{
			methodLinePragmaByFiles.Add(fileID, new HashSet<int>());
		}
		methodLinePragmaByFiles[fileID].Add(methodDeclLine);	
	}
}

//remove declarators from line of MethodDecl
foreach(CxList declarator in declarators)
{
	CSharpGraph declaratorCSG = declarator.data.GetByIndex(0) as CSharpGraph;

	if(declaratorCSG != null && declaratorCSG.LinePragma != null)
	{
		int fileID = declaratorCSG.LinePragma.GetFileId();
		int declaratorLine = declaratorCSG.LinePragma.Line;

		if(methodLinePragmaByFiles.ContainsKey(fileID) && methodLinePragmaByFiles[fileID].Contains(declaratorLine))
		{
			staticDeclaratorsInPrivateMethod -= declarator;
		}
	}
}

result.Add(variableDeclStatic);
result.Add(staticDeclaratorsInPrivateMethod);