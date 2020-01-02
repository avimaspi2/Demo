// Query TOCTOU
// ============
// Time of Check Time Of Use
// The query looks for functions not thread safe and returns the function declaration

CxList nonThreadSafeFunc = All.NewCxList();
CxList methods = Find_Methods();

// code for C
List < string > mutex_lock = new List <string> {"pthread_mutex_lock", "pthread_mutex_trylock", "flock"};

//flags that make open() good for multithread(fail if already open)
CxList O_CREAT = All.FindByShortName("O_CREAT");
CxList O_EXCL = All.FindByShortName("O_EXCL");

//fopen is a vulnerable function
nonThreadSafeFunc.Add(methods.FindByShortName("fopen"));

//For #include <fcntl.h> -> "open", "fcntl" and "creat" is safe if the O_CREAT | O_EXCL is used
CxList fcntlFunctions = methods.FindByShortNames(new List<string> {"open", "fcntl", "creat"});
CxList fcntlFunctionsWithFlags = fcntlFunctions.InfluencedBy(O_CREAT);
fcntlFunctionsWithFlags = fcntlFunctionsWithFlags.InfluencedBy(O_EXCL).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

fcntlFunctions -= fcntlFunctionsWithFlags;
nonThreadSafeFunc.Add(fcntlFunctions);

// code for cpp
List < string > cppfile_streams = new List <string> {"ofstream", "ifstream", "fstream"};
List < string > cpp_mutex = new List <string> {"mutex", "recursive_mutex", "timed_mutex", "recursive_timed_mutex"};

CxList cppFileStream = All.FindByTypes(cppfile_streams.ToArray());
nonThreadSafeFunc.Add(cppFileStream.FindByType(typeof (Declarator)));

CxList vulnFopen = All.NewCxList();

//If there is a use of mutex in same function then it is thread safe
foreach(CxList func in nonThreadSafeFunc)
{
	//get all elements in function
	CxList fopenMethod = func.GetAncOfType(typeof(MethodDecl));
	CxList allInMethod = All.GetByMethod(fopenMethod);
	
	//check if mutex used
	CxList cppMutex = allInMethod.FindByTypes(cpp_mutex.ToArray()).GetMembersOfTarget();

	CxList mutex = allInMethod.FindByShortNames(mutex_lock);	
	mutex.Add(cppMutex.FindByMemberAccess(".lock"));
	mutex.Add(cppMutex.FindByMemberAccess(".try_lock"));

	if (mutex.Count == 0)
	{
		vulnFopen.Add(func);
	}
}

result = vulnFopen;