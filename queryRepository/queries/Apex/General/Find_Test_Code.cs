CxList isTest = All.FindByCustomAttribute("istest").GetFathers();
CxList testClasses = isTest.FindByType(typeof(ClassDecl)) + isTest.FindByType(typeof(MethodDecl));
CxList testMethods = All.FindByFieldAttributes(Modifiers.TestMethod);

CxList testCode = All.GetByAncs(testMethods + testClasses);

CxList testCodePlus = All.NewCxList();
int levels = 0; // "Emergency" exit, in case of an unplanned endless loop
do
{
	CxList nonTestCode = All - testCode;
	CxList nonTestMethods = nonTestCode.FindByType(typeof(MethodDecl));

	CxList nonTestCalledByNonTest = nonTestCode.FindAllReferences(nonTestMethods) - nonTestMethods;
	nonTestCalledByNonTest = nonTestCode.FindDefinition(nonTestCalledByNonTest);

	CxList nonTestCalledByTest = testCode.FindAllReferences(nonTestMethods) - nonTestMethods;
	nonTestCalledByTest = nonTestCode.FindDefinition(nonTestCalledByTest);

	testCodePlus = All.GetByAncs(nonTestCalledByTest - nonTestCalledByNonTest);
	testCode.Add(testCodePlus);
	levels++;
}
while ((testCodePlus.Count > 0) && (levels < 7));

result = testCode;