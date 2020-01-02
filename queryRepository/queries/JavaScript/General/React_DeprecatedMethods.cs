List <string> deprecatedMethods = new List<string>{ "findDOMNode", "renderToString", "renderToStaticMarkup",
		"unmountComponentAtNode", "getDOMNode", "render"};
	
List <string> deprecatedMemberAccess = new List<string> {"getDOMNode",
		"renderToStaticMarkup","renderToString","findDOMNode",
		"ReactEventListener"};
	
List <string> deprecatedMembersOfReactClass = new List <string> {
		"getDOMNode","setProps","replaceProps", "findDOMNode",
		"renderToString", "renderToStaticMarkup"};

CxList reactRefs = React_Find_References();
CxList deprecatedCalls = reactRefs.GetMembersOfTarget().FindByShortNames(deprecatedMethods);
deprecatedCalls.Add(reactRefs.GetMembersOfTarget().FindByShortNames(deprecatedMemberAccess));

CxList deprecatedClassMethdosCalls = Find_Methods().FindByShortNames(deprecatedMembersOfReactClass);
deprecatedCalls.Add(deprecatedClassMethdosCalls);

	// ADDONS
CxList batchedUpdates = All.FindByMemberAccess("addons.batchedUpdates");
CxList cloneWithProps = All.FindByMemberAccess("addons.cloneWithProps");
	
deprecatedCalls.Add(batchedUpdates);
deprecatedCalls.Add(cloneWithProps);
	
	
result.Add(deprecatedCalls);