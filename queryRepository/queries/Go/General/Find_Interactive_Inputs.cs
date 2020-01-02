// from https://golang.org/pkg/bufio/ package
// Package bufio implements buffered I/O.
CxList bufioInputs = All.NewCxList();
List<string> bufioReaders = new List<string> {"NewScanner","NewReader","NewReaderSize"};
CxList bufioInputVariables = All.FindByMemberAccess("\"bufio\".*").FindByShortNames(bufioReaders).GetAssignee();
CxList variablesOcurrences = All.FindAllReferences(bufioInputVariables);

String[] methods = new string[] {"Bytes", "Text","ReadString","ReadRune"}; 
foreach(String m in methods){
	bufioInputs.Add(variablesOcurrences.GetMembersOfTarget().FindByShortName(m));
}

// from https://golang.org/pkg/bytes/ package
// Package bytes implements functions for the manipulation of byte slices.
CxList bytesInputs = All.NewCxList();
List<string> bytesBuffers = new List<string> {"NewBuffer","NewBufferString"};
CxList bytesInputVariables = All.FindByMemberAccess("\"bytes\".*").FindByShortNames(bytesBuffers).GetAssignee();
variablesOcurrences = All.FindAllReferences(bytesInputVariables);

methods = new string[] {"ReadString", "ReadRune"};
foreach(String m in methods){
	bytesInputs.Add(variablesOcurrences.GetMembersOfTarget().FindByShortName(m));
}

// from https://golang.org/pkg/fmt/ package
// This package implements formatted I/O with functions analogous to C
CxList fmtInputs = All.NewCxList();
List<string> scanners = new List<string> {"Scan","Scanf","Scanln","Fscan","Fscanf","Fscanln","Sscan","Sscanf","Sscanln"};
fmtInputs = All.FindByMemberAccess("\"fmt\".*").FindByShortNames(scanners);

result.Add(bufioInputs);
result.Add(bytesInputs);
result.Add(fmtInputs);