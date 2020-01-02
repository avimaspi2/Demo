CxList methods = Find_JQuery_Methods();

//1) add all event binding methods
List<string> eventsList = new List<string>{
		//"bind", // bind methods are added separately
		"blur",
		"change",
		"click",
		"contextmenu",
		"dbclick",
		"dblclick",
		"delegate", 	// Note: .delegate, is deprecated since Ver.3.0
		"die",			// Note: .live, is deprecated since Ver.1.7
		"error",		// Note: .error,is deprecated since Ver.1.8
		"focus",
		"focusin",
		"focusout",
		"hover",
		"jQueryholdReady",
		"jQueryproxy",
		"jQueryready",
		"keydown",
		"keypress",
		"keyup",
		"live",		// Note: .live, is deprecated since Ver.1.7
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
		"unbind",		// Note: .unbind is deprecated since Ver.3.0
		"undelegate",	// Note: .undelegate is deprecated since Ver.3.0
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

//3) add .attr method that accepts an event name as the first parameter.
CxList attrMethods = methods.FindByShortName("attr");

// find all variables passed to .bind method as a first parameter
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

CxList eventString = strings.FindByShortNames(onEventList);
eventString.Add(mobileInit);

// find those that are assigned a string literal which is an event name
assign = All.FindAllReferences(firstParam).FindByAssignmentSide(CxList.AssignmentSide.Left);
// get their reference's which is the first parameter of .bind method
stringVariable = firstParam.FindAllReferences(assign.FindByFathers(eventString.GetFathers()));
stringVariable.Add(eventString);

// add attr methods that recieve an event name string (literal/variable) as their first parameter,
// which could be used later to invoke a handler function on triggering an event trigger
result.Add(attrMethods.FindByParameters(stringVariable));