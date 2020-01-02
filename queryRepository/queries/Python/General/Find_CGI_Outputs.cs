CxList imports = Find_Imports();
string[] cgiMethods = new string[] {"print_environ","print_form", "print_directory", "print_environ_usage()", "test"};

result = Find_Methods_By_Import("cgi", cgiMethods, imports);