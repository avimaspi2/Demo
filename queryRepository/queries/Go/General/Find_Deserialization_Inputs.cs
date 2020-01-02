// Get all the inputs
CxList inputs = Find_Inputs();
// because there is no Flow between when a bytes.Buffer reads from an input, 
// so we need to add the bytes.Buffer structs that read from a input
inputs.Add(Find_Buffer_Readers());
// Because Find_Inputs() only returns the methods file.Read and sometimes this
// method is not even called
inputs.Add(All.FindByMemberAccess("\"os\".*").FindByShortNames(new List<string>{"Open","OpenFile"}));

// The method file.Read(b) parameter is also a input because it contains data 
// read from the file, so we need to find File_Read arguments references
CxList readRefs = Find_UnknownReferences()
	.FindAllReferences(All.GetParameters(Find_Read()));

inputs.Add(readRefs);

// To obtain the longest flows
CxList desserializers = Find_Deserializers_Constructors();
CxList methods = Find_Methods();
CxList parameters = All.GetParameters(methods).InfluencedBy(inputs);
CxList deserializersInitObjects = desserializers.FindByParameters(parameters).GetAssignee(); 
CxList irrelevantInputs = deserializersInitObjects;
irrelevantInputs.Add(desserializers.FindByParameters(parameters));
irrelevantInputs.Add(All.FindAllReferences(deserializersInitObjects));
irrelevantInputs.Add(parameters);

result = inputs - irrelevantInputs;