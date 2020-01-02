CxList methods = Find_Methods();
//Location/document sources:
result = Find_Win_Elem_Address();
result.Add(Find_Members("navigate.href"));
result.Add(Find_Members("location.search"));
result.Add(Find_Members("document.referrer"));
result.Add(Find_Members("document.URL"));
result.Add(Find_Members("document.documentURI"));
result.Add(Find_Members("document.baseURI"));
result.Add(Find_Members("document.URLUnencoded"));
result.Add(Angular_Find_Inputs());
result.Add(Find_Inputs_Knockout());

result -= result.GetTargetOfMembers();

//History sources (search only for history.pushState - doesn't check for pointers to history objects):
CxList historySources = methods.FindByShortNames(new List<string>{"pushState","replaceState"});
CxList histTargets = historySources.GetTargetOfMembers().FindByShortName("history");
result.Add(histTargets.GetMembersOfTarget() * historySources);

result.Add(All.FindByName("*Request.Form"));
result.Add(methods.FindByShortName("InputBox"));
/*result.Add(methods.FindByShortName("prompt"));*/
result.Add(All.FindByMemberAccess("Stdin.Read*"));
result.Add(All.FindByName("UserInput"));
result.Add(All.FindByName("GetUserInput"));
result.Add(Find_Web_Messaging_Inputs());

if(All.GetQueryParam<bool>("isDesktopApp", false) || cxScan.IsFrameworkActive("SAPUI")){
	result.Add(Find_MsAjax_Inputs());
	result.Add(Find_XHR_Response());
	result.Add(Find_SAPUI_Inputs());
	result.Add(Find_SAPUI_OData_Reads());
}

/*
This part is added in order to remove inputs that appear on the
left side of an assignment
*/ 
CxList left = Find_Assign_Lefts();
CxList target = All.NewCxList();
for (int i = 10; i > 0; i--)
{
	if (left.Count <= 0)
	{
		break;
	}
	target.Add(left * result);
	left = left.GetTargetOfMembers();
}
result -= target;
CxList mofTargets = result.GetMembersOfTarget();
result.Add(mofTargets);
result -= mofTargets.GetTargetOfMembers();

result.Add(Add_Get_Attribute(result));
result.Add(AngularJS_Find_Inputs());
result.Add(Find_SAPUI_Inputs());

CxList anonyMethod = Find_LambdaExpr();
//host and hostname not considered as input
CxList toRemove = Find_Members("location.host");
toRemove.Add(Find_Members("location.hostname"));

toRemove.Add(anonyMethod);

result -= toRemove;

result.Add(Find_HTTP_Requests());

result.Add(ReactNative_Find_Inputs());
// Fix limitation: no flow from reference to its members
CxList refs = Find_UnknownReference().FindAllReferences(result);
result.Add(refs.GetRightmostMember());