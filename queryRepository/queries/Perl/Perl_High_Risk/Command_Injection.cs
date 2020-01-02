/// <summary>
/// Find Command Injections
/// Results go from inputs to command execution statements
/// </summary>
CxList inputs = Find_Interactive_Inputs();
CxList commands = Find_Command_Injection();
CxList sanitize = Find_Command_Injection_Sanitizer();

result = commands.InfluencedByAndNotSanitized(inputs, sanitize);