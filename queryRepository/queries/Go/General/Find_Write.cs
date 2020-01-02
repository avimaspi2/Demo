// Find_Write
CxList sttWrites = All.NewCxList();
CxList objWrites = All.NewCxList();

// Static Methods from https://golang.org/pkg/io
sttWrites.Add(All.FindByMemberAccess("\"io\".WriteString"));

// Static Methods from https://golang.org/pkg/io/ioutil/
List<string> ioutilMethods = new List<string> {"WriteFile","TempFile"};
sttWrites.Add(All.FindByMemberAccess("\"io/ioutil\".*").FindByShortNames(ioutilMethods));

// Instances created to write to files, from:
// https://golang.org/pkg/bufio/
// https://golang.org/pkg/os/
CxList varOcurrences = All.NewCxList();
varOcurrences.Add(All.FindByMemberAccess("\"bufio\".NewWriter").GetAssignee());
List<string> osMethods = new List<string> {"Create","NewFile","Open","OpenFile"};
varOcurrences.Add(All.FindByMemberAccess("\"os\".*").FindByShortNames(osMethods).GetAssignee());

// Places where the file is written
CxList references = All.NewCxList();
references.Add(All.FindAllReferences(varOcurrences));
objWrites.Add(references.GetMembersOfTarget().FindByShortNames(new List<string> {"Write*","Flush"}));

result.Add(sttWrites);
result.Add(objWrites);