//Methods that can suffer from Cross-site scripting

CxList Methods = Find_JQuery_Methods();

result = Methods.FindByShortNames(new List<string>{
		"html","append","appendTo","after","before",
		"insertAfter","insertBefore","prepend",
		"prependTo","replaceAll","replaceWith",
		/*"unwrap",*/"wrap","wrapAll","wrapInner"});

// Identify .attr("textContent", <param>)
CxList attrMethods = Methods.FindByShortName("attr");
CxList attrArgs = attrMethods.FindByParameterValue(0, "textContent", Checkmarx.Dom.BinaryOperator.IdentityEquality);
attrArgs = attrArgs.FindByParameterValue(1, "", Checkmarx.Dom.BinaryOperator.IdentityInequality);

result.Add(attrArgs);


// search css styles that can contains url
Methods = Methods.FindByShortName("css");

CxList ur = Find_UnknownReference();

CxList possibleOutputs = ur.FindByShortNames(new List<string>{
		"background","behavior","content","cue","cue-after",
		"cue-before","include-source","layer-background-image",
		"list-style-image","play-during"}, false);

result.Add(
	Methods.DataInfluencedBy(possibleOutputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));