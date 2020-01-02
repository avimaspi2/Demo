CxList methods = Find_Methods();

CxList otherCommandsWithException = methods.FindByMemberAccess("System.loadLibrary");

// java.lang.ref.Reference.clone() throws CloneNotSupportedException
CxList referenceClone = methods.FindByMemberAccess("Reference.clone");

// java.lang.Class.getAnnotation() throws TypeNotPresentException when the annotation class is not present
CxList classGetAnnotation = methods.FindByMemberAccess("Class.getAnnotation");

CxList IO = Find_IO();

CxList propertyMembers = All.FindByMemberAccess("Properties.*");
IO.Add(propertyMembers.FindByMemberAccess("Properties.load"));
IO.Add(propertyMembers.FindByMemberAccess("Properties.loadFromXML"));
IO.Add(propertyMembers.FindByMemberAccess("Properties.store"));
IO.Add(propertyMembers.FindByMemberAccess("Properties.storeToXML"));

//From all methods of IO we should exclude the following cases:
// 1. Any calls to StringWriter.write* since they don't throw exceptions 
// 2. Any calls to PrintWriter.print* except the overloaded function that receives as parameter char array ( the only method that throws exception) 
 
CxList Excluded_IO = methods.FindByMemberAccess("StringWriter.write*");
Excluded_IO.Add(methods.FindByMemberAccess("PrintWriter.write*"));

CxList printWriter = All.FindByMemberAccess("PrintWriter.print*");
//Find all char array declarators
CxList declarator = All.FindByType("char").FindByType(typeof(Declarator)); 
declarator = base.Find_RankSpecifier().GetByAncs(declarator.GetAncOfType(typeof(VariableDeclStmt)));
declarator = declarator.GetAncOfType(typeof(Declarator)) - declarator; 
//For each method in printWriter CxList check in the parameter list if it has one parameter and its type is char array to exclude it from the safe function list  
foreach(CxList item in printWriter){
	CxList parameters = All.GetParameters(item) - All.GetParameters(item).FindByType(typeof(Param)); 
	parameters = parameters.FindAllReferences(declarator); 
	if( 1 == parameters.Count ) {
		printWriter -= parameters.GetAncOfType(typeof(MethodInvokeExpr));  
	}
}

Excluded_IO.Add(printWriter);
Excluded_IO.Add(propertyMembers.FindByMemberAccess("Properties.getProperty")); // This won't throw an exception
result.Add(IO - Excluded_IO);

//javax.management.MBeanOperationInfo constructor with 6 params
CxList newObjects = Find_Object_Create();
CxList relevantMBeanOperationInfoConstructor = newObjects.FindByShortName("MBeanOperationInfo");
CxList sixthParamOfConstructor = All.GetParameters(relevantMBeanOperationInfoConstructor, 5);
relevantMBeanOperationInfoConstructor = relevantMBeanOperationInfoConstructor.FindByParameters(sixthParamOfConstructor);

// java.net.URLClassLoader constructors
CxList urlClassLoaderConstructors = newObjects.FindByShortName("URLClassLoader");
	
result.Add(Find_FileSystem_Read());
result.Add(Find_FileSystem_Write());
result.Add(Find_FileSystem_Sanity_Checks());
result.Add(Find_DB());
result.Add(Find_Security_Methods_Throwing_Exceptions());
result.Add(relevantMBeanOperationInfoConstructor);
result.Add(urlClassLoaderConstructors);
result.Add(otherCommandsWithException);
result.Add(referenceClone);
result.Add(classGetAnnotation);

result -= result.GetParameters(result); // leave only commands, not parameters
result -= result.GetByAncs(methods.FindByShortName("getAttribute")); // remove "2nd-level" inputs and DB
result -= Find_Properties_Files();