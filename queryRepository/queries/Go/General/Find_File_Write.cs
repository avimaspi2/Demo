// Find_File_Write
CxList writes = All.NewCxList();
writes.Add(Find_Write());

// Parameters of methods that accept the Writer interface
// This interface wraps the basic Write method that can be used to write to files
CxList methodsWithWriters = All.NewCxList();
CxList writerParams = All.NewCxList();
methodsWithWriters.Add(All.FindByMemberAccess("\"io\".WriteString"));
methodsWithWriters.Add(All.FindByMemberAccess("\"fmt\".Fprint*"));
writerParams.Add(All.GetParameters(methodsWithWriters,0));

// All WriteString that uses in the first parameter *File as a Writer
CxList readFileMeth = All.NewCxList();
CxList readFileAssg = All.NewCxList();
CxList readFileRefs = All.NewCxList();
readFileMeth.Add(All.FindByMemberAccess("\"os\".*").FindByShortNames(new List<string>{"Open", "OpenFile"}));
readFileAssg.Add(readFileMeth.GetFathers().GetAssignee()); // GetFathers is needed due to multiple returns
readFileRefs.Add(writerParams.FindAllReferences(readFileAssg));

// The Difference between the previous selection and all Writes
// resulting only the writes to files
CxList diff = All.NewCxList();
diff.Add(( writerParams - readFileRefs.GetFathers() ).FindByType(typeof(Param)));
diff = diff.GetAncOfType(typeof(MethodInvokeExpr));
result.Add(writes - diff);