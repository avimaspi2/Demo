// from https://golang.org/pkg/flag/ package
// Package implements command-line flag parsing.
CxList flagInputs = All.NewCxList();
List<string> methods = new List<string> {"String","Int","Bool","StringVar","Args","Arg"};
flagInputs = All.FindByMemberAccess("\"flag\".*").FindByShortNames(methods);

// from https://golang.org/pkg/os/ package
// Package that provides basic interfaces to I/O primitives.
CxList osInputs = All.NewCxList();
methods = new List<string> {"Getenv","Args"};
osInputs = All.FindByMemberAccess("\"os\".*").FindByShortNames(methods);

result.Add(flagInputs);
result.Add(osInputs);