CxList methods = Find_Methods();

// All the relevant (basic) methods
CxList fgets = methods.FindByShortName("fgets");
CxList fscanf = methods.FindByShortName("fscanf");
CxList fgetc = methods.FindByShortName("fgetc");
CxList fread = methods.FindByShortName("fread");

List<string> readMethodsNames = new List<string> {"read","pread","pread64","readlink" };
CxList read = methods.FindByShortNames(readMethodsNames);

CxList regQueryValue = methods.FindByShortName("RegQueryValueEx"); //3,4,5
regQueryValue.Add(methods.FindByShortName("RegQueryValue"));
CxList regGetValue = methods.FindByShortName("RegGetValue"); //4,5,6

// Get basic input methods and their relevant parameters
CxList fgetsFread = All.NewCxList();
fgetsFread.Add(fgets, fread);

CxList inputMethods = All.NewCxList();
inputMethods.Add(fgets, fscanf, fgetc);
inputMethods.Add(All.GetParameters(fgetsFread, 0));
inputMethods.Add(All.GetParameters(read, 1));
inputMethods.Add(All.GetParameters(regQueryValue, 3));
inputMethods.Add(All.GetParameters(regQueryValue, 4));
inputMethods.Add(All.GetParameters(regQueryValue, 5));
inputMethods.Add(All.GetParameters(regGetValue, 4));
inputMethods.Add(All.GetParameters(regGetValue, 5));
inputMethods.Add(All.GetParameters(regGetValue, 6));

CxList fscanfParamsToRemove = All.GetParameters(fscanf);
fscanfParamsToRemove.Add(All.GetParameters(fscanf, 0));
fscanfParamsToRemove.Add(All.GetParameters(fscanf, 1));

inputMethods -= fscanfParamsToRemove;

// Add stream methods
CxList inGet = methods.FindByMemberAccess("fstream.get");
inGet.Add(methods.FindByMemberAccess("ifstream.get"));

CxList inGetline = methods.FindByMemberAccess("fstream.getline");
inGetline.Add(methods.FindByMemberAccess("ifstream.getline"));

CxList inGetGetline = All.NewCxList();
inGetGetline.Add(inGet, inGetline);

inGet.Add(All.GetParameters(inGetGetline, 0));

CxList inPeek = methods.FindByMemberAccess("fstream.peek");
inPeek.Add(methods.FindByMemberAccess("ifstream.peek"));

CxList inRead = methods.FindByMemberAccess("fstream.read*");
inRead.Add(methods.FindByMemberAccess("ifstream.read*"));

inRead = All.GetParameters(inRead, 0);

CxList inPutBack = methods.FindByMemberAccess("fstream.putback");
inPutBack.Add(methods.FindByMemberAccess("ifstream.putback"));

inPutBack = All.GetParameters(inRead, 0);

CxList inSbumpc = All.GetParameters(methods.FindByMemberAccess("filebuf.sbumpc"), 0);
CxList inSgetc = methods.FindByMemberAccess("filebuf.sgetc");
CxList inSgetn = All.GetParameters(methods.FindByMemberAccess("filebuf.sgetn"), 0);
CxList inSnextc = methods.FindByMemberAccess("filebuf.snextc");
CxList inSputbackc = All.GetParameters(methods.FindByMemberAccess("filebuf.sputbackc"), 0);

result.Add(inputMethods, inGet, inPeek, inRead, inPutBack, inSbumpc, inSgetc, inSgetn, inSnextc, inSputbackc);