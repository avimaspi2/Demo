//	Find Read
//  ---------
//  Find all the inputs from a file.
///////////////////////////////////////////////////////////////////////

CxList methods = Find_Methods();

// All the relevant (basic) methods
CxList fgets = methods.FindByShortNames(new List<string> {"fgets","fgetws"});
CxList fscanf = methods.FindByShortName("fscanf");
CxList fgetc = methods.FindByShortName("fgetc");
CxList fread = methods.FindByShortName("fread");
CxList read = methods.FindByShortName("read")
	+ methods.FindByShortName("pread")
	+ methods.FindByShortName("pread64")
	+ methods.FindByShortName("readlink");
CxList regQueryValue = methods.FindByShortName("RegQueryValueEx"); //3,4,5
regQueryValue.Add(methods.FindByShortName("RegQueryValue"));
CxList regGetValue = methods.FindByShortName("RegGetValue"); //4,5,6

// Get basic input methods and their relevant parameters
result = fgets + fscanf + fgetc +
	All.GetParameters(fgets + fread, 0) + 
	// All but 1st and 2nd params of fscanf:
	All.GetParameters(fscanf) - All.GetParameters(fscanf, 0) - All.GetParameters(fscanf, 1) +
	All.GetParameters(read, 1) + //the second param of read/pread is input
	// registry values:
	All.GetParameters(regQueryValue, 3) + All.GetParameters(regQueryValue, 4) + 
	All.GetParameters(regQueryValue, 5) + 
	All.GetParameters(regGetValue, 4) + All.GetParameters(regGetValue, 5) +
	All.GetParameters(regGetValue, 6);


// Add stream methods
CxList inGet = methods.FindByMemberAccess("fstream.get")
	+ methods.FindByMemberAccess("ifstream.get");
CxList inGetline = methods.FindByMemberAccess("fstream.getline")
	+ methods.FindByMemberAccess("ifstream.getline");
inGet.Add(All.GetParameters(inGet + inGetline, 0));

CxList inPeek = methods.FindByMemberAccess("fstream.peek")
	+ methods.FindByMemberAccess("ifstream.peek");

CxList inRead = methods.FindByMemberAccess("fstream.read*")
	+ methods.FindByMemberAccess("ifstream.read*");
inRead = All.GetParameters(inRead, 0);

CxList inPutBack = methods.FindByMemberAccess("fstream.putback")
	+ methods.FindByMemberAccess("ifstream.putback");
inPutBack = All.GetParameters(inRead, 0);

CxList inSbumpc = All.GetParameters(methods.FindByMemberAccess("filebuf.sbumpc"), 0);
CxList inSgetc = methods.FindByMemberAccess("filebuf.sgetc");
CxList inSgetn = All.GetParameters(methods.FindByMemberAccess("filebuf.sgetn"), 0);
CxList inSnextc = methods.FindByMemberAccess("filebuf.snextc");
CxList inSputbackc = All.GetParameters(methods.FindByMemberAccess("filebuf.sputbackc"), 0);

result.Add(inGet + inPeek + inRead + inPutBack
	+ inSbumpc + inSgetc + inSgetn + inSnextc + inSputbackc);