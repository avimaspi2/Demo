//Functions & procedures which have definer rights privileges either by AUTHID DEFINER clause or by default
//if there is no AUTHID clause in thier declaration.
//This issue can be solved by adding AUTHID CURRENT_USER clause to the procedure/function declaration. 

CxList methods = All.FindByType(typeof(MethodDecl));
methods -= methods.FindByShortName("CxMethod_*") + All.FindByFieldAttributes(Modifiers.Private);

//methods which do not have CURRENT_USER rights
CxList current_user = All.FindByCustomAttribute("current_user");
CxList definer_rights_Methods = methods - current_user.GetAncOfType(typeof(MethodDecl));

//PL/SQL structures which do not have AUTHID clause
CxList otherAttributes = 
	All.FindByCustomAttribute("cursor") +
	All.FindByCustomAttribute("with");

CxList otherStructures = otherAttributes.GetAncOfType(typeof(MethodDecl));
otherStructures.Add(All.FindByReturnType("view")+
					All.FindByReturnType("table")+
					All.FindByReturnType("sequence"));

//stored procedures/functions which exist inside PL/SQL package/object should have CURRENT_USER rights in their
//father package/object and thus we do not alert in this query
CxList fatherClass = All.FindByType(typeof(ClassDecl));
fatherClass -= fatherClass.FindByShortName("DefaultClass");
CxList methodsNotUnderPackageObject = definer_rights_Methods.GetByAncs(fatherClass);


result = definer_rights_Methods - methodsNotUnderPackageObject - otherStructures;