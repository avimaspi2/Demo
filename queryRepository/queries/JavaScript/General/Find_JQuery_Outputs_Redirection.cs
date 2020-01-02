//Methods that can redirect user to another url

CxList Methods = Find_JQuery_Methods();

// Identify .attr("attr-name", <param>) where attr-name is href.
CxList attrMethods = Methods.FindByShortName("attr");

CxList attrArgs = attrMethods.FindByParameterValue(0, "href", Checkmarx.Dom.BinaryOperator.IdentityEquality);
attrArgs = attrArgs.FindByParameterValue(1, "", Checkmarx.Dom.BinaryOperator.IdentityInequality);

result.Add(attrArgs);