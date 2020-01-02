//DoS by Unreleased Resources Query
//---------------------------------

// Find all stream declaration from following format:
//		ifstream v1;
//  	ifstream v2("c:\1.txt");
//		ofstream outStrm;
CxList streamDecl = Find_All_Declarators().FindByType("ifstream")+
		            Find_All_Declarators().FindByType("ofstream");
// The statment like 
//		ifstream v1;
// just define the variable "v1", but statement like
//      	ifstream v2("c:\1.txt");
// open the connection 
// this declaration should be stored for monitoring to be sure that it's closed by v2.close() statement
// In the DOM the statemnt 
//			ifstream v2("c:\1.txt"); 
// is stored as: VariableDeclStmt -> Declarators -> Declarator -> ObjectCreateExpr -> Param
// The block below looks for this kind of declarators
CxList variableDeclStatement = Find_Param().GetAncOfType(typeof(ObjectCreateExpr)).GetFathers();
streamDecl = streamDecl.GetByAncs(variableDeclStatement).FindByType(typeof(Declarator));

CxList methods = Find_Methods();
CxList io = (methods.FindByMemberAccess("filebuf.open") + 
	methods.FindByMemberAccess("fstream.open") + 
	methods.FindByMemberAccess("ifstream.open") + 
	methods.FindByMemberAccess("ofstream.open")).GetTargetOfMembers();

//Found all places where file is open
CxList fopen = Find_Open_Files_Methods() + io;

//found "left side" of .open() methods
CxList targetOpen = fopen.GetTargetOfMembers();

//Found defention of the "file" variable that used for open methods
CxList openDef = All.FindDefinition(targetOpen) + streamDecl;

//Found all places where file is closed
CxList fclose = methods.FindByShortName("fclose")
	+ methods.FindByMemberAccess("CFile.Close")
	+ methods.FindByShortName("close");

//found "left side" of .close() methods
CxList targetClose = fclose.GetTargetOfMembers();

//Found defention of the "file" variable that used for open methods
CxList closeDef = All.FindDefinition(targetClose);

//The code below looks for variable definition (file handlers) that was opened at least in one place but not closed never
//Note: actiually, it possable situation that file is opened in one place, but because of if -else controls it 
//should be closed in 2 places
//Current solution does not supports control flow, so not all cases will be handled
CxList notClosed = openDef - closeDef;

//	The file declaration that never closed, look for all "open" places
//	Additionally, if file opened by statement 
//			ifstream v2("c:\1.txt");
//  it is opened without open() statment, therefore in result the Declarator should be returned: (streamDecl - closeDef)
result = All.FindAllReferences(notClosed).GetMembersOfTarget().FindByShortName("open") + (streamDecl - closeDef);