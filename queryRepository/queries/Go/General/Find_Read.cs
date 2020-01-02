// from https://golang.org/pkg/io/ioutil/ package
// Package that provides basic interfaces to I/O primitives.
CxList ioutilInputs = All.FindByMemberAccess("\"io/ioutil\".*").FindByShortNames(new List<string>{"ReadAll", "ReadFile"});
CxList ioInputs = All.FindByMemberAccess("\"io\".*").FindByShortNames(new List<string>{"ReadAtLeast", "ReadFull"});

// from https://golang.org/pkg/os/ package
// Package that provides basic interfaces to I/O primitives.
CxList readInputs = All.NewCxList();
CxList openCalls = All.FindByMemberAccess("\"os\".*").FindByShortNames(new List<string>{"Open", "OpenFile"});
CxList fileVariables = All.DataInfluencedBy(openCalls); // this is gathering a lot of unnecessary information
CxList fileVariablesOcurrences = All.FindAllReferences(fileVariables);

readInputs.Add(fileVariablesOcurrences.GetMembersOfTarget().FindByShortNames(new List<string> {"Read", "ReadAt"}));

result.Add(ioutilInputs);
result.Add(ioInputs);
result.Add(readInputs);