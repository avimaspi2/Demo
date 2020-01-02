CxList methods = Find_JQuery_Methods();

//1) add all event binding methods
List<string> eventsList = new List<string>{
		//"bind", // bind methods are added separately
		"blur",
		"change",
		"click",
		"dbclick",
		"dblclick",
		"delegate",
		"die",
		"error",		// Note: .error,is deprecated since Ver.1.8
		"focus",
		"focusin",
		"focusout",
		"hover",
		"jQueryproxy",
		"keydown",
		"keypress",
		"keyup",
		"live",
		"load",		// Note: .load, is deprecated since Ver.1.8
		"mousedown",
		"mouseenter",
		"mouseleave",
		"mousemove",
		"mouseout",
		"mouseover",
		"mouseup",
		"off",
		"on",
		"one",
		"ready",
		"resize",
		"scroll",
		"select",
		"submit",
		"toggle",		// Note: .toggle is deprecated since Ver.1.8
		"trigger",
		"triggerHandler",
		"unbind",
		"undelegate",
		"unload"};		// Note: .unload is deprecated since Ver.1.8
	// Note: .error, .load, .unload and .toggle are deprecated since Ver.1.8

result.Add(methods.FindByShortNames(eventsList));


//2) distinguish between methods that are not JQuery methods, but has the same name as a JQuery method: {"bind", "error"}
//	 error method has the same syntax for Typescript and JQuery and so cannot be distinguished

CxList bindMethods = methods.FindByShortName("bind");
CxList strings = Find_String_Literal();
CxList unKnown = Find_UnknownReference();

// find all variables passed to .bind method as a first parameter
CxList firstParam = unKnown.GetParameters(bindMethods, 0);

// find those that are assigned a string literal
CxList assign = All.FindAllReferences(firstParam).FindByAssignmentSide(CxList.AssignmentSide.Left);
// get their reference's which is the first parameter of .bind method
CxList stringVariable = firstParam.FindAllReferences(assign.FindByFathers(strings.GetFathers()));
stringVariable.Add(strings.GetParameters(bindMethods, 0));

// add bind methods that recieve a string (literal/variable) as their first parameter, which is the syntax for JQuery only
result.Add(bindMethods.FindByParameters(stringVariable));

//3) add .attr method that accepts an event name, 'value' or 'data-*' as the first parameter.
CxList attrMethods = methods.FindByShortName("attr");

// find all variables passed to .attr method as a first parameter
firstParam = unKnown.GetParameters(attrMethods, 0);

List<string> onEventList = new List<string>{
		"change",
		"click",
		"dbclick",
		"dblclick",
		"error",		// Note: .error,is deprecated since Ver.1.8
		"focus",
		"focusin",
		"focusout",
		"hover",
		"keydown",
		"keypress",
		"keyup",
		"load",		// Note: .load, is deprecated since Ver.1.8
		"mousedown",
		"mouseenter",
		"mouseleave",
		"mousemove",
		"mouseout",
		"mouseover",
		"mouseup",		
		"ready",
		"resize",
		"scroll",
		"select",
		"submit",
		"toggle", // Note: .toggle is deprecated since Ver.1.8
		};

// mobileInit 
CxList mobileInit = strings.FindByShortName("mobileinit");
if (mobileInit.Count > 0)
{
	onEventList.AddRange(new List<string> {
			"hashchange", // jQuery Mobile
			"navigate", // jQuery Mobile
			"orientationchange", // jQuery Mobile
			"pagebeforechange", // jQuery Mobile
			"pagebeforecreate", // jQuery Mobile
			"pagebeforehide", // jQuery Mobile
			"pagebeforeload", // jQuery Mobile
			"pagebeforeshow", // jQuery Mobile
			"pagechange", // jQuery Mobile
			"pagechangefailed", // jQuery Mobile
			"pagecreate", // jQuery Mobile
			"pagehide", // jQuery Mobile
			"pageinit", // jQuery Mobile
			"pageload", // jQuery Mobile
			"pageloadfailed", // jQuery Mobile
			"pageremove", // jQuery Mobile
			"pageshow", // jQuery Mobile
			"scrollstart", // jQuery Mobile
			"scrollstop", // jQuery Mobile
			"swipe", // jQuery Mobile
			"swipeleft", // jQuery Mobile
			"swiperight", // jQuery Mobile
			"tap", // jQuery Mobile
			"taphold", // jQuery Mobile
			"throttledresize", // jQueryMobile
			"updatelayout", // jQueryMobile
			"vclick", // jQueryMobile
			"vmousecancel", // jQueryMobile
			"vmousedown", // jQueryMobile
			"vmousemove", // jQueryMobile
			"vmouseout", // jQueryMobile
			"vmouseover", // jQueryMobile
			"vmouseup" // jQueryMobile
			});
}
//Find Event string
CxList eventString = strings.FindByShortNames(onEventList);
eventString.Add(mobileInit);

//Find 'value' or 'data-*' string
List<string> paramsList = new List<string>{"value","data-*"};
CxList otherParams = strings.FindByShortNames(paramsList);

// find those that are assigned a string literal which is an event name, 'value' or 'data-*' 
assign = All.FindAllReferences(firstParam).FindByAssignmentSide(CxList.AssignmentSide.Left);
// get event name reference's which is the first parameter of .attr method
stringVariable = firstParam.FindAllReferences(assign.FindByFathers(eventString.GetFathers()));
stringVariable.Add(eventString);
// get 'value' or 'data-*' reference's which is also the first parameter of .attr method
CxList paramVariable = firstParam.FindAllReferences(assign.FindByFathers(otherParams.GetFathers()));
paramVariable.Add(otherParams);

//Heuristic to catch page forms inputs
CxList value = All.FindByShortName("value");
CxList PotentialFormElem = All.FindByShortNames(new List<string>{"*element*","*form*","*options*", "body"}, false);
PotentialFormElem = PotentialFormElem.GetMembersOfTarget();
PotentialFormElem.Add(PotentialFormElem.GetMembersOfTarget());
result.Add(PotentialFormElem * value);
result.Add(value.GetParameters(Find_Methods().FindByShortName("getAttribute"), 0));

//add .attr methods that recieve an event name string (literal/variable) as their first parameter,
// which could be used later to invoke a handler function on triggering an event trigger
result.Add(attrMethods.FindByParameters(stringVariable));
//Also add .attr get methods that receive 'value' or 'data-*' string (literal/variable) as their first parameter
CxList attrGets = attrMethods.FindByParameters(paramVariable);
//Remove .attr setters that receive 'value' or 'data-*' string (literal/variable) as their first parameter
CxList attrSetters = attrGets.FindByParameters(All.GetParameters(attrGets, 1));
attrGets -= attrSetters; 
result.Add(attrGets);

CxList potentialDomElements = Find_Potential_DOM_Elements();
result.Add(potentialDomElements.FindByMemberAccess("*.value"));

//4) add innerText, text and nodeValue Dom properties
CxList inputs = Find_MemberAccesses().FindByShortNames(
	new List<string>{"innerText","text","nodeValue"});

//Remove inputs that appear on left side
CxList assignLeftSide = Find_Assign_Lefts();
inputs -= inputs * Find_Assign_Lefts();
inputs -= inputs.GetTargetsWithMembers(assignLeftSide, 10);

//Remove text setters
CxList setText = (inputs * Find_Methods()).FindByShortName("text");
setText = setText.FindByParameters(All.GetParameters(setText, 1));
inputs -= setText;

result.Add(inputs);