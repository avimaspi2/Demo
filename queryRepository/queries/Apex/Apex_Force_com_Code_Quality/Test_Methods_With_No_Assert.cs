CxList testCode = Find_Test_Code();
CxList classDecl = testCode.FindByType(typeof(ClassDecl));	// all class decls in test code
CxList testClasses = testCode.FindByCustomAttribute("istest").GetAncOfType(typeof(ClassDecl));
															// all @isTest classes (in test code)
CxList nonTestClasses = classDecl - testClasses;			// classes under test code that are not @isTest
testCode -= testCode.GetByAncs(nonTestClasses);

CxList testMethods = testCode.FindByType(typeof(MethodDecl));

// Remove the test utility methods (just static methods in test classes that don't have @isTest or testmethod modifier).
CxList isTest = All.FindByCustomAttribute("istest").GetFathers();
testMethods = testMethods.FindByFieldAttributes(Modifiers.TestMethod) + (testMethods * isTest);

CxList methodInvoke = All.FindByType(typeof(MethodInvokeExpr));
CxList assertMethods = methodInvoke.FindByMemberAccess("System.assert", false)
	+ methodInvoke.FindByMemberAccess("System.assertEquals", false)
	+ methodInvoke.FindByMemberAccess("System.assertNotEquals", false);

// Check also calls to assert methods
int numAssert = 0;
for(int i = 0; i < 10 && assertMethods.Count > numAssert; i++)
{
	numAssert = assertMethods.Count;
	assertMethods.Add(All.FindAllReferences(assertMethods.GetAncOfType(typeof(MethodDecl))));
}

CxList noAssert = testMethods - assertMethods;

CxList noAssertArtificial = noAssert.FindByShortName("get*");
noAssertArtificial = 
	noAssertArtificial.FindByShortName("getid") + 
	noAssertArtificial.FindByShortName("getisdeleted") + 
	noAssertArtificial.FindByShortName("getcreatedbyid") + 
	noAssertArtificial.FindByShortName("getcreateddate") + 
	noAssertArtificial.FindByShortName("getlastmodifiedbyid") + 
	noAssertArtificial.FindByShortName("getlastmodifieddate") + 
	noAssertArtificial.FindByShortName("getsystemmodstamp") + 
	noAssertArtificial.FindByShortName("getname") + 
	noAssertArtificial.FindByShortName("getlastmodifiedby") + 
	noAssertArtificial.FindByShortName("getowner") + 
	noAssertArtificial.FindByShortName("getownerid") + 
	noAssertArtificial.FindByShortName("getcreatedby");

CxList testCodeInNoAssertArtificial = testCode.GetByAncs(noAssertArtificial);
foreach (CxList method in noAssertArtificial)
{
	if (testCodeInNoAssertArtificial.GetByAncs(method).Count < 10)
	{
		noAssert -= method;
	}
}

CxList noAssertTestReferences = testCode.FindAllReferences(noAssert) - noAssert;
CxList noAssertNoTestReferences = (All - testCode).FindAllReferences(noAssert) - noAssert;
CxList calledOnlyFromTest = testCode.FindDefinition(noAssertTestReferences) - testCode.FindDefinition(noAssertNoTestReferences);

result = noAssert - calledOnlyFromTest;