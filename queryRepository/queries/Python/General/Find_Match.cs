CxList imports = Find_Imports();

string[] matchMethods = new string[] {"match", "search", "findall"};

result = Find_Methods_By_Import("re", matchMethods, imports);