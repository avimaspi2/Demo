///////////////////////////////////////////////////////////////////////
// Unprotected_Cookie
// A cookie should be set with httponly and secure definitions 
// which can come in the form of:
//      - anonymous function in the cookie definition
//      - a property of a cookie
//      - a string in the response header
///////////////////////////////////////////////////////////////////////

//find "secure" and "httponly" that set to true
CxList classes = Find_Classes();
CxList unknown = Find_UnknownReference();
CxList assignLeft = Find_Assign_Lefts();

CxList unRef = assignLeft.FindByType(typeof(UnknownReference));

CxList trueLiteral = Find_BooleanLiteral().FindByShortName("true");
CxList assignedTrue = trueLiteral.GetAssignee();
CxList httpOnlyTrue = assignedTrue.FindByShortName("httpOnly");
CxList secureTrue = assignedTrue.FindByShortName("secure");


//get only the anonymous methods where httponly and secure are set to true 
CxList safeObjects = All.NewCxList();
foreach (CxList classD in classes)
{
	if ( (httpOnlyTrue.GetByAncs(classD).Count > 0) && (secureTrue.GetByAncs(classD).Count > 0) )
	{
		safeObjects.Add(classD);
	}
}
CxList methods = Find_Methods();
CxList safeAnony = methods.GetByAncs(safeObjects).FindByShortName(safeObjects);

//find where cookies are set 
CxList source = methods.FindByShortNames(new List<string>{"cookie", "cookies", "setCookie"}, false);  
CxList cookies = unknown.FindByShortNames(new List<string>{"cookie", "cookies"}, false);

//methods that not interact directly with cookie itself
List <string> sanitizeList = new List<string>{"indexOf"};
CxList sanitizedOperations = methods.FindByShortNames(sanitizeList);
cookies -= (cookies.GetRightmostMember() * sanitizedOperations).GetLeftmostTarget();

CxList documentCookie = Find_MemberAccesses().FindByShortName("cookie", false);
CxList cookiesRightMostMember = cookies.GetRightmostMember();
source.Add(cookiesRightMostMember.GetAssignee(unRef));
source.Add(cookiesRightMostMember.GetFathers().GetAssignee(unRef)); 
source.Add(documentCookie.FindByAssignmentSide(CxList.AssignmentSide.Left));

CxList sourceRemove = source.FindByShortName("cookieParser");	// cookiep parser is commonly used with Express framework
sourceRemove.Add(source.FindByType(typeof(MethodDecl)));
sourceRemove.Add(source.FindByType(typeof(Declarator)));
source -= sourceRemove;

//find cookie definitions that are not safe (not secure and httponly) 
CxList safe = source.DataInfluencedBy(safeAnony);
safe.Add(safeAnony);
//get cookie declaration instead of var definition
CxList potential = source - safe;
potential.Add(source.FindByType(typeof(MethodInvokeExpr)));
potential -= safe;
potential -= source.DataInfluencedBy(safe);

// Look for: var cookie;/var cookie = {}; cookie[httpOnly] = true; cookie[secure] = true;
CxList members = All.NewCxList();
CxList httpOnlyParam = All.NewCxList();
CxList secureParam = All.NewCxList();
CxList temp = All.NewCxList();
CxList tempMethod = All.NewCxList(); 
foreach (CxList potentialResult in potential)
{
	temp = All.NewCxList();
	temp.Add(potentialResult);
	// If potentialResult is an anony-object - seek the variable assigend to it to check it sanitised through the variable	
	// If potentialResult is of type MethodDecl of ClassDecl - seek the UnknownReference through the MethodInvoke object
	if (potentialResult.FindByType(typeof(MethodDecl)).Count > 0)
	{
		tempMethod = methods.FindAllReferences(potentialResult);
	}
	else if (potentialResult.FindByType(typeof(ClassDecl)).Count > 0)
	{
		tempMethod = methods.GetByAncs(potentialResult).FindByShortName(potentialResult);
	}	
	temp = unRef.FindByFathers(tempMethod.GetFathers());
	
	members = unknown.FindAllReferences(temp).GetMembersOfTarget();
	httpOnlyParam = members * httpOnlyTrue;
	secureParam = members * secureTrue;
	if ( (httpOnlyParam.Count > 0) && (secureParam.Count > 0) )
	{
		safe.Add(potentialResult);
		safe.Add(temp);
		safe.Add(tempMethod);
	}
}

result = potential - safe;
result = result.ReduceFlowByPragma();


//find a setHeader function of Cookie-set that isnt influenced by httponly and secure
CxList res = All.FindByShortNames(new List<string>{"res", "response"});
CxList setHeader = res.GetMembersOfTarget().FindByShortName("setHeader");
CxList setHeaderCookie = setHeader.FindByParameters(All.GetParameters(setHeader, 0).FindByShortName("Set-Cookie", false));
CxList strings = Find_String_Literal();
CxList httponly = strings.FindByShortName("*httpOnly*", false);
CxList secure = strings.FindByShortName("*secure*", false); 

CxList httpOnlyInflu = setHeaderCookie.InfluencedBy(httponly);
CxList bothInflu = httpOnlyInflu.InfluencedBy(secure);
       
result.Add(setHeaderCookie - bothInflu);

CxList toRemove = result.FindByType(typeof(ClassDecl));
toRemove.Add(result.FindByType(typeof(MethodDecl)));
//remove class and method declarations, they don't make sense appear
result -= toRemove;

if(Hapi_Find_Server_Instance().Count > 0)
{
	result.Add(Hapi_Find_Unprotected_Cookie());
}