CxList methods = Find_Methods();

CxList password = Find_Password_In_NewConnection();
//Another way to read from a filestream 
CxList propload = methods.FindByMemberAccess("Properties.load");
//There are several classes that act as fileinputstream:
CxList fileinputstr = All.FindByType("FileInputStream");
fileinputstr.Add(methods.FindByMemberAccess("Files.newBufferedReader"));
fileinputstr.Add(methods.FindByMemberAccess("Files.newByteChannel"));
fileinputstr.Add(methods.FindByMemberAccess("Files.newDirectoryStream"));
fileinputstr.Add(methods.FindByMemberAccess("Files.newInputStream"));
fileinputstr.Add(All.FindByType("FileReader"));

CxList proploadfromfile = propload.FindByParameters(fileinputstr);
//Included gettargetofmembers because there is no flow from load to its target
CxList inputs = Find_Read();
inputs.Add(proploadfromfile);
inputs.Add(proploadfromfile.GetTargetOfMembers());

CxList sanitize = Find_General_Sanitize();
 

// All passwords of getConnection that are affected by a non-interactive input, and not well sanitized
result = inputs.InfluencingOnAndNotSanitized(password, sanitize);