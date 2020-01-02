//Find_not_Null_Termination_String
//http://msdn.microsoft.com/en-US/library/d5t46152(v=VS.80).aspx
CxList methods = Find_Methods();

var methodNotNullTerminationString = new List<string>{
		"read",
		"pread",
		"pread64",
		"readlink",
		"readsome",
		"_Read_s",
		"_Readsome_s"
		};

CxList methodsIndList = methods.FindByShortNames(methodNotNullTerminationString);

result = All.GetParameters(methodsIndList);