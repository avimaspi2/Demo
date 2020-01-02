/*
	Download_of_Code_Without_Integrity_Check
	This query looks for code that loads a library without a checksum test
	before.

	We start by identifying all the locations where an external library
	is loaded.  From here we extract all the relevant inputs to them: file
	paths and/or binary content.  Once we have a list of relevant inputs,
	we filter those that are used in a checksum procedure.  We then filter
	further by those that are used in a comparison procedure, in order to
	verify that their checksums are actually being compared to something.
	So far we have a list of inputs whose checksum is calculated and compared
	to something.  Next we determine the If statements that are influenced 
	by this	list.  Finally, the results are the locations where an external
	library	is being loaded not under the influence of such If statements, in
	order to verify that their loading actually depends upon a checksum.
*/

// Auxiliary (constant) information
CxList methods = Find_Methods();
CxList checksumMethods = Find_Weak_Hashes();
checksumMethods.Add(methods.FindByShortName("md5Hex"));

CxList comparisonMethods = methods.FindByShortName("equals");
CxList declarators = Find_Declarators();

// Identify the locations where an external library is being loaded and
// the relevant inputs to those methods.
CxList loadingPoints = All.NewCxList();
CxList inputs = All.NewCxList();

CxList temp = methods.FindByName("System.load");
temp.Add(methods.FindByName("System.loadLibrary"));

inputs.Add(declarators.FindDefinition(All.GetParameters(temp, 0)));
loadingPoints.Add(temp);

temp = methods.FindByName("ClassLoader.defineClass");
inputs.Add(declarators.FindDefinition(All.GetParameters(temp, 1)));
loadingPoints.Add(temp);

//Class.forName(String className)
CxList userInputs = Find_Inputs();
CxList classes = methods.FindByMemberAccess("Class.forName");
CxList classesParams = All.GetParameters(classes, 0);
CxList forNameUserInputParam = classesParams * All.FindAllReferences(userInputs);
forNameUserInputParam.Add(classesParams.DataInfluencedBy(userInputs));
CxList forImportant = classes.FindByParameters(forNameUserInputParam);
loadingPoints.Add(forImportant);

// The methods above take the binary name of a class.  We can't really
// test if they're checksummed a priori.  We play safe and opt to signal
// them anyway.
loadingPoints.Add(methods.FindByMemberAccess("ClassLoader.findClass"));
loadingPoints.Add(methods.FindByMemberAccess("ClassLoader.findSystemClass"));
loadingPoints.Add(methods.FindByMemberAccess("ClassLoader.loadClass"));
loadingPoints.Add(Find_Object_Create().FindByType("DexClassLoader"));

// Identify checksum methods that are being passed those inputs.
CxList checksumPoints = checksumMethods.InfluencedBy(inputs);

// Identify comparison methods that are being passed those checksum results.
CxList comparisonPoints = comparisonMethods.InfluencedBy(checksumPoints);

// Identify If statements whose branches are influenced by those comparison results.
CxList branchingPoints = All.InfluencedBy(comparisonPoints).GetAncOfType(typeof(IfStmt));

// Identify loading points that are under the influence of those branching points.
CxList sanitizedLoadingPoints = loadingPoints.GetByAncs(branchingPoints);

result = loadingPoints - sanitizedLoadingPoints;