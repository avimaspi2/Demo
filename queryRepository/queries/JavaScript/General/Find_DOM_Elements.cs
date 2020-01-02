CxList methods = Find_Methods();
CxList vars = Find_UnknownReference();
vars.Add(Find_Declarators());
CxList memberAccesses = Find_MemberAccesses();
vars.Add(memberAccesses);

List<string> domGetters = new List<string> {
		"activeElement",
		"evaluate",
		"getAttributeNode",
		"getElementsByClassName",
		"getElementById",
		"getElementsByName",
		"getElementsByTag",
		"getElementsByTagName",
		"getElementsByTagNameNS",
		"querySelector",
		"querySelectorAll"
		};

List < string > propertiesElems = new List<string> {
		"body",
		"documentElement",
		"firstElementChild",
		"lastElementChild",
		"nextElementSibling",
		"head",
		"parentElement",
		"previousElementSibling",
		"newPostForm"        // WordPress forums and alike
		};

List < string > propertiesList = new List<string> {
		"anchors",
		"applets",
		"childNodes",
		"children",
		"embeds",
		"forms",
		"images",
		"links",
		"scripts"
		};

CxList domGettersList = methods.FindByShortNames(domGetters);
CxList propListElements = memberAccesses.FindByShortNames(propertiesList);

CxList getters = methods.FindByShortNames(new List<string> {"createElement","getElementById"});
getters.Add(domGettersList);
getters.Add(domGettersList.GetFathers().FindByType(typeof(MemberAccess)));
getters.Add(propListElements.GetFathers().FindByType(typeof(MemberAccess)));
getters.Add(memberAccesses.FindByShortNames(propertiesElems));

//check if propListElements is decendent of DOM element
foreach(CxList prop in propListElements)
{
	CxList propFather = prop.Clone();

	for(int i = 0; i < 10; ++i)
	{
		if (propFather.Count <= 0)
		{
			break;
		}
		CxList propInGetters = getters * propFather;

		if(propInGetters.Count > 0)
		{
			getters.Add(prop);
			break;
		}
		propFather = propFather.GetTargetOfMembers();
	}
}      
    

//Search for 
//document.getElementById();
result = vars.FindAllReferences(getters.GetTargetOfMembers());
CxList varDecl = Find_Assign_Lefts();

//Search for
//a = document.getElementById();
//or
//var a = document.getElementById();
//or
//a = document.getElementById().getElementById();
//or
//var a = document.getElementById().getElementById();
CxList getterFathers = getters.GetFathers();
CxList fathersDecl = getterFathers.FindByType(typeof(Declarator));
CxList fathersAssign = getterFathers.FindByType(typeof(AssignExpr));

CxList leftSideGetter = All.NewCxList();
leftSideGetter.Add(fathersDecl);
leftSideGetter.Add(varDecl.FindByFathers(fathersAssign));
result.Add(vars.FindAllReferences(leftSideGetter) - leftSideGetter);

//Search for
//document.getElementById().getElementById();
//and
//document.getElementById();
CxList fathersDeclAssign = All.NewCxList();
fathersDeclAssign.Add(fathersDecl);
fathersDeclAssign.Add(fathersAssign);		

CxList fathersWithoutAssign = getterFathers - fathersDeclAssign;
result.Add(getters.FindByFathers(fathersWithoutAssign));

//JQuery Getters
//$("tag").[9];
//$("tag").get(var);
//var int_var =1; $("tag").[int_var];
CxList jqueyMethods = Find_JQuery_Methods();
CxList jqueryMA = jqueyMethods.FindByType(typeof(MemberAccess));
result.Add(jqueyMethods.FindByShortName("get"));
result.Add(jqueryMA - jqueryMA.FindByShortName("length"));