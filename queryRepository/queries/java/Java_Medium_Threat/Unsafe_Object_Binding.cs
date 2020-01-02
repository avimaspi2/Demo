/*
Mass assignement can accidently expose unintended objects or attribute.
This query for Spring MVC, will find all 

Find RequestMapped object 
Look at member objects of the above object
Find member object setters that are public
Return member object that are publicly set, if it is not primitive – because primitives cannot be chained into sub-objects (IE User object allows manipulating its own Details object)

*/
//general inititation for optimization reasons
CxList potentiallyVuln = All.NewCxList();
CxList typeref = Find_TypeRef();
CxList classDecl = Find_ClassDecl();
CxList methodDecl = Find_MethodDeclaration();
CxList declarator = Find_Declarators();
CxList customAttribute = Find_CustomAttribute();

Modifiers mod = new Modifiers();
mod = Dom.Modifiers.Public;

//find Request Mapped object that is being passed to the model.

CxList rm = customAttribute.FindByShortName("RequestMapping");
CxList potentialMethod = rm.GetAncOfType(typeof(MethodDecl));
CxList customAtt = customAttribute.FindByShortName("ModelAttribute");
CxList parameters = All.GetParameters(potentialMethod);
foreach(CxList prm in parameters)
{
	if(customAtt.GetByAncs(prm).Count > 0)
	{
		potentiallyVuln.Add(prm);
	}
} 


//find all public setters
CxList publicMethods = methodDecl.FindByFieldAttributes(mod);
CxList setters = publicMethods.FindByShortName("set*", false);
/*
make sure setters are indeed setting
*/


CxList classProperty = Find_FieldDecls();
CxList allReferencesOfClassProperty= All.FindAllReferences( classProperty);
CxList thisRef = All.FindByType(typeof(ThisRef));

CxList thisRefRefer = All.NewCxList();
thisRefRefer.Add(thisRef.GetMembersOfTarget());
thisRefRefer.Add(allReferencesOfClassProperty);

CxList thisRefLeftOfAssign = thisRefRefer.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList indeedSetters = All.NewCxList();
foreach(CxList setter in setters)
{
	if(thisRefLeftOfAssign.GetByAncs(setter).Count > 0)
	{
		indeedSetters.Add(setter);
	}
}



//find only members of objects that are set in a public method and are not primitive
ArrayList types = new ArrayList();
types.Add("string");
types.Add("String");
types.Add("byte");
types.Add("char");
types.Add("short");
types.Add("int");
types.Add("Integer");
types.Add("long");
types.Add("float");
types.Add("double");
types.Add("boolean");
String [] array = (String[]) types.ToArray(typeof( string ));
CxList primitives = All.FindByTypes(array);

foreach(CxList obj in potentiallyVuln)
{
	CxList tr = typeref.GetByAncs(obj);
	CxList classFound = classDecl.FindAllReferences(tr);
	CxList foundSetter = indeedSetters.GetByAncs(classFound);         
	CxList whatIsSet = thisRefLeftOfAssign.GetByAncs(foundSetter);
	CxList definitionFound = (declarator.FindDefinition(whatIsSet)); 
	definitionFound -= primitives;
	result.Add(obj.ConcatenateAllTargets(definitionFound));
                
}