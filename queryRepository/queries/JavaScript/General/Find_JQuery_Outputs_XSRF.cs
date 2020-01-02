//Methods that can suffer from Cross-site request forgery

CxList Methods = Find_JQuery_Methods();

// Identify .attr("attr-name", <param>) where attr-name are action, src and href.
CxList attrMethods = Methods.FindByShortName("attr");
CxList attrArgs = attrMethods.FindByParameterValue(0, "action", Checkmarx.Dom.BinaryOperator.IdentityEquality);
attrArgs.Add(attrMethods.FindByParameterValue(0, "src", Checkmarx.Dom.BinaryOperator.IdentityEquality));
attrArgs.Add(attrMethods.FindByParameterValue(0, "href", Checkmarx.Dom.BinaryOperator.IdentityEquality));
attrArgs = attrArgs.FindByParameterValue(1, "", Checkmarx.Dom.BinaryOperator.IdentityInequality);

result.Add(attrArgs);


//needs to check jquery $.ajax support
CxList urlTypes = Find_UnknownReference();
urlTypes.Add(Find_String_Literal());
urlTypes.Add(Find_MemberAccesses());
CxList ajax = Methods.FindByShortNames(new List<string>{"get","post"});
CxList ajaxUrls = urlTypes.GetParameters(ajax, 0).DataInfluencingOn(ajax);

ajax.Add(Methods.FindByShortNames(new List<string>{"ajax"}));
ajaxUrls.Add(Find_Declarators().FindByShortName("url").DataInfluencingOn(ajax));

result.Add(ajaxUrls);