/// <summary>
/// Find outputs to the console. There are few sources for these outputs:
/// 1) sys - stdout.write, stderr.write, displayhook, excepthook
/// 2) builtin - print
/// 3) traceback - 
/// </summary>

// Add sys output methods
string[] sys = new string[] {"stdout", "stderr"};
CxList stdoutMethods = Find_Methods_By_Import("sys", sys);
CxList stdoutWrite = stdoutMethods.GetMembersOfTarget().FindByShortName("write");
result.Add(stdoutWrite);

string[] hooks = new string[] {"displayhook", "excepthook"};
CxList hookMethods = Find_Methods_By_Import("sys", hooks);
result.Add(hookMethods);

// Find builtin methods
result.Add(Find_Methods().FindByShortName("print"));

// Find Traceback methods
string[] tracebacks = new string[] {"print_tb", "print_exception", "print_exc", "print_last", "print_stack"};
CxList tbMethods = Find_Methods_By_Import("traceback", tracebacks);
result.Add(tbMethods);