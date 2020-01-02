// Query Methods_Without_ReturnType
// ////////////////////////////////
// The purpose of this query is to find the functions/methods that are not defined with any type.
// Example:
// foo(){ } should be recognized as bad code practice
// ==============================================================================================

// The methodDecls variable will keep the list of all Method Declarations
CxList methodDecls = Find_Method_Declarations();

// The method that has no returned type defined such as
// "foo(){ }" is kept in the DOM like "int foo(){ }"
CxList intMethodDecls = methodDecls.FindByReturnType("int");

//There are 4 types of method declaration selected so far:
// 1) int f1() 	 	- return type is kept in DOM as int
// 2) f2()   		- return type is kept in DOM as int
// 3) int * f3()   	- return type is kept in DOM as int*
// 4) int ** f4()   - return type is kept in DOM as int**
//
// From the list above, only f2() should be selected.
foreach(CxList intMethodDecl in intMethodDecls)
{
	MethodDecl methodDeclDom = intMethodDecl.TryGetCSharpGraph<MethodDecl>();	
	TypeRef returnTypeDom = methodDeclDom.ReturnType; 
	
	// The line pragma of the method declaration f2() will be SAME as the line pragma of its return type, 
	// since "int" return type of f2() is not explicit.
	if (methodDeclDom.LinePragma.CompareTo(returnTypeDom.LinePragma) == 0)
	{
		result.Add(intMethodDecl);
	}
}