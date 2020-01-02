CxList methods = Find_Methods();

CxList write = Find_Write();
CxList output = Find_Outputs();
CxList strings = Find_Strings();

CxList parameters = strings.GetParameters(write) + strings.GetParameters(output);

CxList writeStrings = methods.FindByParameters(parameters);
write = (write + output) - writeStrings;

result = Find_Log_Outputs() + 
	write 
	;