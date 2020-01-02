// Find_File_Read

// All *File references implements the Reader and Writer Interface
// Instances created to open files, from:
// https://golang.org/pkg/os/
CxList openFileMeth = All.NewCxList();
CxList openFileAssg = All.NewCxList();
CxList openFileRefs = All.NewCxList();
openFileMeth.Add(All.FindByMemberAccess("\"os\".*").FindByShortNames(new List<string>{"Open", "OpenFile"}));
openFileAssg.Add(openFileMeth.GetFathers().GetAssignee()); // GetFathers is needed due to multiple returns
openFileRefs.Add(All.FindAllReferences(openFileAssg));

// Direct calls to Open
result.Add(openFileRefs.GetMembersOfTarget().FindByShortNames(new List<string> {"Read", "ReadAt"}));

// ReadFile comes from a file
result.Add(All.FindByMemberAccess("\"io/ioutil\".ReadFile"));

// All Static Methods that accept as the first parameter 
// a Reader that comes from a *File handler
CxList readers = All.NewCxList();
readers.Add(All.FindByMemberAccess("\"fmt\".*").FindByShortNames(new List<string>{"Fscan","Fscanf","Fscanln"}));
readers.Add(All.FindByMemberAccess("\"io\".*").FindByShortNames(new List<string>{"ReadAtLeast", "ReadFull"}));
readers.Add(All.FindByMemberAccess("\"io/ioutil\".ReadAll"));

// Other methods with a Reader in the first parameter
CxList readersWithAssign = All.NewCxList();
readersWithAssign.Add(All.FindByMemberAccess("\"bufio\".*").FindByShortNames(new List<string>{"NewScanner","NewReader","NewReaderSize"})); // Has Assignees

CxList parameters = All.NewCxList();
CxList paramsOpenFile = All.NewCxList();
CxList methOpenFile = All.NewCxList();

// First Parameters of Methods that accept a Reader in that parameter
parameters.Add(All.GetParameters(readers, 0));

// All parameters with a Reader that comes from an Open File operation
// or a reader
CxList assignements = All.NewCxList();
assignements.Add(openFileAssg);
assignements.Add(readersWithAssign.GetAssignee());
paramsOpenFile.Add(parameters.FindAllReferences(assignements));

// The methods used by the previous filtered parameters
methOpenFile.Add(paramsOpenFile.GetAncOfType(typeof(MethodInvokeExpr)));

result.Add(methOpenFile);

CxList readersAssg = All.NewCxList();
CxList readersRefs = All.NewCxList();
CxList refOpenFile = All.NewCxList();
readersAssg.Add(readersWithAssign.GetAssignee());
readersRefs.Add(All.FindAllReferences(readersAssg));
refOpenFile.Add(All.FindAllReferences(methOpenFile.GetAssignee()));

result.Add(refOpenFile.GetMembersOfTarget().FindByShortNames(new List<string> {"Bytes", "Text","ReadString","ReadRune"}));