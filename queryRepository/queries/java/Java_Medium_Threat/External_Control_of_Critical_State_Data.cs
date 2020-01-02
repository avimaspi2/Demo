/*
Here we find any permission that is set by an input.
We will deal with 3 different cases:
1. The simplest - a direct (data) inlfuence of the input on the permission
2. A bit more complex - a control influence of an input on the permission
3. The most complex case - a control influence of a variable, 
						   that was control influenced by an input, 
						   on the permission
Since we currently don't have control influence, we will "simulate" it by checking "if" statement conditions.
*/

// General variables
CxList getCookies = All.FindByMemberAccess("request.getCookies");
getCookies.Add(Find_CookieValue_Annotation());
CxList input = Find_Inputs() - getCookies;
CxList permissions = All.FindByMemberAccess("Permissions.add");
CxList conditions = Find_Conditions();


/// 1 - a direct (data) inlfuence of the input on the permission
CxList permissionsByInput = permissions.DataInfluencedBy(input);


/// 2 - a control influence of an input on the permission

// All if-statements that contain permission updates
CxList permissionsCheck = permissions.GetAncOfType(typeof(IfStmt));
// The conditions of these if-statements
CxList permissionsCheckCondition = conditions.GetByAncs(permissionsCheck);

// All the conditions in if-statements, that are influenced by an input
CxList checkByInput = permissionsCheckCondition.DataInfluencedBy(input);


/// 3 - a control influence of a variable, that was control influenced by an input, on the permission

// All the references of the if-conditions that contain permission updates
CxList permissionsCheckRef = All.FindAllReferences(All.GetByAncs(permissionsCheckCondition));
	//permissionsCheckRef = permissionsCheckRef - conditions;
	//return permissionsCheckRef - conditions - permissionsCheckRef.GetByAncs(conditions.GetAncOfType(typeof(IfStmt)));
// The if statements, containing these references
CxList permissionsIf = permissionsCheckRef.GetAncOfType(typeof(IfStmt));
// All the conditions of these if-statements, that are "getValue.equals", i.e. comparing the value
// of an input to something
CxList permissionCondition = All.GetByAncs(conditions.GetByAncs(permissionsIf)).FindByMemberAccess("getValue.equals");

// Find the input-based permissions, tat are actually based on an input
CxList permissionByInput = permissionCondition.DataInfluencedBy(input);
// Look at all the references of the conditions that apply to a permission change, and find the ones
// that are inside an if-statement, affected by an input
permissionsCheckRef = permissionsCheckRef.GetByAncs(permissionByInput.GetAncOfType(typeof(IfStmt)));

// This additional influence is needed in order to show the influence path of the second if-statement
// condition (containing the input condition) on the first if-statement condition (seting the
// permission)
CxList controlInfluenceByInput = permissionsCheckRef.DataInfluencingOn(permissionsCheckCondition);


/// The final result - all the relevant cases 
result.Add(permissionsByInput);
result.Add(checkByInput);
result.Add(controlInfluenceByInput);