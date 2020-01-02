result.Add(Angular_Find_Outputs_XSS());

CxList commands = Find_Members("document.create");
commands.Add(Find_Members("document.createElement"));
commands.Add(Find_Members("document.write"));
commands.Add(Find_Members("document.writeln")); 
commands.Add(Find_Members("*.createContextualFragment"));
commands.Add(Find_Members("ActiveXObject.loadXML"));	// addition of ActiveXObject.loadXML

result.Add(Find_Members("document.URLUnencoded"));
result.Add(Find_Members("document.body"));
result.Add(Find_Members("style.background"));
result.Add(Find_Members("style.behavior"));
result.Add(Find_Members("style.content"));
result.Add(Find_Members("style.cue"));
result.Add(Find_Members("style.cue-after"));
result.Add(Find_Members("style.cue-before"));
result.Add(Find_Members("style.include-source"));
result.Add(Find_Members("style.layer-background-image"));
result.Add(Find_Members("style.list-style-image"));
result.Add(Find_Members("style.play-during"));

CxList commandParameters = All.GetParameters(commands);	
CxList onlyParams = commandParameters.FindByType(typeof(Param));
foreach(CxList cmd in onlyParams)
{
	CxList invoke = cmd.GetAncOfType(typeof(MethodInvokeExpr));
	result.Add((commandParameters - onlyParams).GetByAncs(cmd).ConcatenateAllTargets(invoke));
}

CxList frameworkOutputs = Find_Framework_Outputs();
frameworkOutputs.Add(Find_JQuery_Outputs_XSS());
frameworkOutputs -= Find_Handlebars_Sanitize();
result.Add(frameworkOutputs);

//DOM HTML element modifiers
CxList domProperties = Find_Members("innerHTML");
domProperties.Add(Find_Members("outerHTML"));

result.Add(Find_Members("appendChild"));
result.Add(Find_Members("insertBefore"));
result.Add(Find_Members("insertAdjacentHTML"));

result.Add(domProperties);

//ES6 Reflection API
//Find usage of Reflect.set with dangerous parameters
domProperties.Add(Find_Members("location"));
domProperties.Add(Find_Members("href"));
CxList reflectSet = All.FindByMemberAccess("Reflect.set");
CxList propertyKey = All.GetParameters(reflectSet, 1);
CxList dangerousProperties = propertyKey.InfluencedBy(Find_Inputs_NoWindowLocation());
dangerousProperties.Add(propertyKey * domProperties);
reflectSet = reflectSet.FindByParameters(dangerousProperties);
result.Add(All.GetParameters(reflectSet, 2));
result.Add(AngularJS_Find_Outputs_XSS());
result.Add(Find_SAPUI_Outputs_XSS());
result.Add(React_Find_XSS_Outputs());