// Incorrect_Permission_Assignment_For_Critical_Resources
// This query looking for places where software specifies permissions for a security-critical resource (files and directories) 
// in a way that allows that resource to be read or modified by unintended actors.
// Atention! function "chmod" and "umask" changing permisions in way, that can't be simply analized,
// so we will find this function as varnability for human's analize.
CxList SuspMethod = All.NewCxList();
CxList AllParams = Find_Param();
CxList methods = Find_Methods();
CxList unknownRef = Find_Unknown_References();
// 1.1 -> find the use of chmod() command.
// this is potentionaly BAD code, becase it change permitions to files to unknown set
CxList chmodList = methods.FindByShortNames(new List<string> {"_chmod","chmod"});
result = chmodList;
// 1.2 -> find the use of system("chmod 755 yourExeFile")
CxList systemList = methods.FindByShortName("system");
CxList systemParam = AllParams.GetParameters(systemList);
CxList badSystensCalls = systemParam.FindByShortName("*chmod*");
result.Add(badSystensCalls);
//2 -> find the use of _umask & _umask_s
CxList umaskList = methods.FindByShortNames(new List<string> {"_umask","_umask_s"});
result.Add(umaskList);
badSystensCalls = systemParam.FindByShortName("*umask*");
result.Add(badSystensCalls);

//3 -> Find File Creations
// 3.1 Find File CreateExpr as @ofstream TruePos ("example.txt")@, but clean @ofstream FalsePos;@
CxList fileFolderDecl = All.FindByTypes(new string [] {
	"FILE", "FileStream",
	"fstream", "ofstream", "std::fstream", "std::ofstream", "ostringstream", 
	"path", "boost::filesystem::path"});
CxList objectCreateExp = fileFolderDecl.FindByType(typeof(ObjectCreateExpr));
SuspMethod.Add( objectCreateExp.FindByParameters(AllParams.GetParameters(objectCreateExp)));

//3.2 FILE x = open("x.txt"); BUT NOT "int fd = open (port);"
string open = "*open";
CxList allOpens = methods.FindByShortName(open);
List < string > vulnerableMethodNames = new List<string>
	{"open", "_open", "_wopen", "fopen", "_wfopen"}; 

CxList vulnerableMethod = allOpens.FindByShortNames(vulnerableMethodNames);
CxList rightSide = vulnerableMethod.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList t2 = rightSide.GetFathers(); // can be AssignExpr or Declarator from left side of "="
CxList t3 = t2.FindByType(typeof(AssignExpr));
t2 = t2 - t3; // left at t2 all that NOT AssignExpr 

//find all Suspect for FILE&Co declarations
CxList PathFiles = fileFolderDecl.FindByType(typeof(UnknownReference)) + fileFolderDecl.FindByType(typeof(Declarator));
//find the left side of Assigment with vulnerableMethod on right
CxList leftSide_t3 = PathFiles.GetByAncs(t3).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList leftSide = leftSide_t3 + t2* PathFiles;
SuspMethod.Add(leftSide);

//3.3  std::fstream fs_w;
//     fs_w.open("test.txt", std::fstream::in);
// in following code creating file with default permission
CxList fileMemberAccess = methods.FindByMemberAccess("FILE.*");
fileMemberAccess.Add(methods.FindByMemberAccess("fstream.*"));
fileMemberAccess.Add(methods.FindByMemberAccess("ofstream.*"));
fileMemberAccess.Add(methods.FindByMemberAccess("path.*"));
vulnerableMethodNames = new List<string> { "move", "copy", "open"}; 
vulnerableMethod = fileMemberAccess.FindByShortNames(vulnerableMethodNames);
SuspMethod.Add(vulnerableMethod);

//4 -> Find writing functions
//4.1 Old style
vulnerableMethodNames = new List<string>
	{"create_directory", "create_directories", "mkdir", "_mkdir"}; 
vulnerableMethod = methods.FindByShortNames(vulnerableMethodNames);
SuspMethod.Add(vulnerableMethod);

//4.2 New Style ( with attribute)
vulnerableMethodNames = new List<string>
	{"CreateDirectory*", "CreateFile*", "CreateFileTransacted*"}; 
vulnerableMethod = methods.FindByShortNames(vulnerableMethodNames);
//return all potentionaly vulnerableMethod;

foreach( CxList sm in vulnerableMethod)
{
	CxList smParam = AllParams.GetParameters(sm);
	if(smParam.Count < 2 ) // only one param - name of file -> no SECURITY_ATTRIBUTES
	{	SuspMethod.Add(sm);}
	else if(smParam.Count == 2 && // CreateDirectory("C:\\Kevin",NULL)// NULL = SECURITY_ATTRIBUTES
		(AllParams.GetParameters(sm, 1).GetName().ToLower().Equals("null")))
	{	SuspMethod.Add(sm);}
	else if(smParam.Count == 3 && // 
		(AllParams.GetParameters(sm, 2).GetName().ToLower().Equals("null")))
	{	SuspMethod.Add(sm);}
	else if(smParam.Count >= 4 && //CreateFile(TEXT("two.txt"),FILE_APPEND_DATA,FILE_SHARE_READ,NULL,... // no security   
		(AllParams.GetParameters(sm, 3).GetName().ToLower().Equals("null")))
	{	SuspMethod.Add(sm);}
}
//Add CopyFile  - no possibility to add SECURITY_ATTRIBUTES 
SuspMethod.Add(methods.FindByShortName("CopyFile"));
//SuspMethod - all methods without  SECURITY_ATTRIBUTES

//Sanitizer : SetFileAttributes
//clean all sanitithed params
CxList Sanitizers = methods.FindByShortName("SetFileAttributes");
CxList paramOfSanitizers = AllParams.GetParameters(Sanitizers);// This is Params
CxList fathersParamOfSanitizers = All.FindByFathers(paramOfSanitizers);// We don't need the Param, but fathers
fathersParamOfSanitizers.Add(All.FindAllReferences(fathersParamOfSanitizers));// and all references of them

CxList paramOfSuspMethod = AllParams.GetParameters(SuspMethod);
CxList fathersParamOfSuspMethod = All.FindByFathers(paramOfSuspMethod);

CxList SanStrLit = fathersParamOfSanitizers.FindByType(typeof(StringLiteral));

bool FlagSL = (SanStrLit.Count > 0);
foreach( CxList sm in SuspMethod)
{
    CxList thisMethodParamsFth = fathersParamOfSuspMethod.FindByFathers(paramOfSuspMethod.GetParameters(sm));
	CxList same = thisMethodParamsFth * fathersParamOfSanitizers;
	if(same.Count > 0)
	{
		continue;
	}
	else if(FlagSL) // we have string literal params for Sanitizer
	{
		CxList strLit = thisMethodParamsFth.FindByType(typeof(StringLiteral));
		CxList found = strLit.FindByShortName(SanStrLit);
		if(found.Count > 0)
		{
			continue;
		}
	}
	result.Add(sm);
}