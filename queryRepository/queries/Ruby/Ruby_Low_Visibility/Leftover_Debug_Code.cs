CxList classes = All.FindByType(typeof(ClassDecl));

//Ruby Test Unit Classes
CxList unitTestClasses = Find_Test_Unit_Code();

//method declaration
CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList mDecl = methodDecl.FindByShortName("test");
mDecl.Add(methodDecl.FindByShortName("test_*"));
result = mDecl - mDecl.GetByAncs(unitTestClasses);

//class declaration
CxList cDecl = classes.FindByShortName("*Test");
cDecl.Add(classes.FindByShortName("Test*"));
result.Add(cDecl - unitTestClasses);

//method call
CxList methods = Find_Methods().FindByShortName("test");
result.Add(methods - methods.GetByAncs(unitTestClasses));

//namespaces
CxList namespaceDecl = All.FindByType(typeof(NamespaceDecl));
result.Add(namespaceDecl.FindByShortName("*test_*"));

//Remove remaining *_test.rb, *_test_helper.rb and test_helper.rb files
result -= result.FindByFileName("*_test.rb");
result -= result.FindByFileName("*test_helper.rb");