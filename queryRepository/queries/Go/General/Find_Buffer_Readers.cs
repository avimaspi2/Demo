CxList unknownRefs = Find_UnknownReferences();

//Get all bytes.Buffers references
CxList readers = Find_Members_By_Import("bytes", new string[] {"Buffer"});
CxList structBuffer = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Buffer");
CxList decls = readers.GetAncOfType(typeof(Declarator));
decls.Add(structBuffer.GetAssignee());
CxList readersRefs = unknownRefs.FindAllReferences(decls);

// Get all the inputs refs
CxList inputs = Find_Inputs();
//Only the method file.Read() is a result from Find_Inputs() and we need to find 
// the open file references 
inputs.Add(Find_File_Open_Refs());
CxList refs = unknownRefs.FindAllReferences(inputs.GetAncOfType(typeof(Declarator)));

// Search ReadFrom methods with a input as a parameter, and get the target member of the methods
// https://golang.org/pkg/io/#ReaderFrom
CxList readFrom = Find_Methods().FindByShortName("ReadFrom");
CxList readFromObj = readFrom.FindByParameters(refs).GetTargetOfMembers();

// The struct that invokes ReadFrom must be from the interface bytes.buffer
result = readersRefs * readFromObj;