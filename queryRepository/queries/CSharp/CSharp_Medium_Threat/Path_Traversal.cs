/*	Path_Traversal
	Finds file system accesses guided by unsanitized pathnames.

	Start by finding as many file system accesses as possible.
	This is accomplished by looking for objects of a certain type
	(like "FileInfo") and for occurrences of certain System.IO
	classes (like "File"). See the `fileIO` variable.

	However, not all methods obtained from this last step are
	relevant, e.g. "Close"/"Dispose". Moreover, even for relevant
	methods there are irrelevant parameters, e.g. in "AppendAllText(
	string path, string content)" the parameter "content" is irrelevant
	with regards to this query's purpose. To cope with this situation,
	we consider these cases as sanitizers, along with the default results
	of Find_Sanitize. See the `sanitizers` variable.

	The output is then all the I/O methods whose pathname is an unsanitized
	result from Find_Interactive_Inputs.
*/

/* Find as many file system accesses as possible */
CxList fileIO = All.NewCxList();
{
	string[] relevantTypes = {
		"DirectoryInfo",		"*.DirectoryInfo",
		"FileInfo",				"*.FileInfo",
		"FileStream",			"*.FileStream",
		"StreamReader",			"*.StreamReader"
		};
	
	List<string> relevantClasses = new List<string> {
			"Directory",
			"File"
			};
	
	CxList objects = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(Declarator));
	
	fileIO.Add(objects.FindByTypes(relevantTypes));
	fileIO.Add(All.FindByShortNames(relevantClasses));
	
	fileIO = fileIO.GetMembersOfTarget(); // We want the methods, not the objects
	fileIO.Add(Find_ASP_MVC_Controller_File_Result());
}

/* Populate the sanitizers */
CxList sanitizers = Find_Sanitize();
//add as sanitizer replace of problematic paths - .. or "/" or "\"
CxList replace = All.FindByName("*.Replace*");

replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

replace = replace.FindByParameterValue(0, "..", BinaryOperator.IdentityEquality) + 
	replace.FindByParameterValue(0, "\\", BinaryOperator.IdentityEquality) +
	replace.FindByParameterValue(0, "/", BinaryOperator.IdentityEquality);

replace -= replace.FindByParameterValue(1, "..", BinaryOperator.IdentityEquality) +
	replace.FindByParameterValue(1, "\\", BinaryOperator.IdentityEquality) + 
	replace.FindByParameterValue(1, "//", BinaryOperator.IdentityEquality);

sanitizers.Add(replace);
{
	// NOTE: These method names are not tied to an object type.
	List<string> irrelevantMethods = new List<string> {
			"Close",
			"Dispose"
			};
	// NOTE: These method names are not tied to an object type.
	Dictionary<string, List<int>> irrelevantParams = new Dictionary<string, List<int>>();
	irrelevantParams.Add("AppendAllLines", new List<int> {1,2});		// File.AppendAllLines(path, _, _);
	irrelevantParams.Add("AppendAllText", new List<int> {1,2});			// File.AppendAllText(path, _, _);
	irrelevantParams.Add("GetAccessControl", new List<int> {1});		// File.GetAccessControl(path, _);
	irrelevantParams.Add("ReadAllLines", new List<int> {1});			// File.ReadAllLines(path, _);
	irrelevantParams.Add("ReadAllText", new List<int> {1});				// File.ReadAllText(path, _);
	irrelevantParams.Add("WriteAllBytes", new List<int> {1});			// File.WriteAllBytes(path, _);
	irrelevantParams.Add("ReadLines", new List<int> {1});				// File.ReadLines(path, _);
	irrelevantParams.Add("WriteAllLines", new List<int> {1,2});			// File.WriteAllLines(path, _, _);
	irrelevantParams.Add("WriteAllText", new List<int> {1,2});			// File.WriteAllText(path, _, _);
	
	irrelevantParams.Add("CreateDirectory", new List<int> {1});			// Directory.CreateDirectory(path, _);
	irrelevantParams.Add("EnumerateDirectories", new List<int> {1,2});	// Directory.EnumerateDirectories(path, _, _);
	irrelevantParams.Add("EnumerateFiles", new List<int> {1,2});		// Directory.EnumerateFiles(path, _, _);
	irrelevantParams.Add("EnumerateFileSystemEntries", new List<int> {1,2});	// Directory.EnumerateFileSystemEntries(path, _, _);
	irrelevantParams.Add("GetDirectories", new List<int> {1,2});		// Directory.GetDirectories(path, _, _);
	irrelevantParams.Add("GetFiles", new List<int> {1,2});				// Directory.GetFiles(path, _, _);
	irrelevantParams.Add("GetFileSystemEntries", new List<int> {1,2});	// Directory.GetFileSystemEntries(path, _, _);
	
	sanitizers.Add(fileIO.FindByShortNames(irrelevantMethods));
	foreach (KeyValuePair<string, List<int>> entry in irrelevantParams)
	{
		CxList m = fileIO.FindByShortName(entry.Key);
		if (m.Count > 0)
			foreach (int i in entry.Value)
				sanitizers.Add(All.GetParameters(m, i));
	}
}

//Add LINQ with IsLetter as sanitizer Find_Methods()
CxList findMethods = Find_Methods();
CxList linqWhereAndSelect = findMethods.FindByShortNames(new List<string> {"where", "select"}, false);
CxList lambdaMethods = findMethods.GetByAncs(All.FindByType(typeof(LambdaExpr)));
CxList isLetterMethod = lambdaMethods.FindByShortName("IsLetter");
CxList whereSanitizers = linqWhereAndSelect.FindByParameters(isLetterMethod.GetByAncs(All.GetParameters(linqWhereAndSelect)).GetAncOfType(typeof(LambdaExpr)));
sanitizers.Add(whereSanitizers);

/* Path Traversal doesn't involve reading from network, but reading from sources like disk  */
sanitizers.Add(All.FindByType("HttpWebResponse"));
// add getFileName as sanitizers to path traversal
sanitizers.Add(findMethods.FindByMemberAccess("Path.GetFileName"));


CxList StreamReaderConstructors = All.FindByShortName("StreamReader").FindByType(typeof(ObjectCreateExpr));
CxList firstParam = All.GetParameters(StreamReaderConstructors, 0);
firstParam = firstParam - firstParam.FindByType("FileStream") - firstParam.FindByType("String", false);
sanitizers.Add(firstParam);

result = fileIO.InfluencedByAndNotSanitized(Find_Interactive_Inputs(), sanitizers);