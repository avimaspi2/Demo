CxList commands = Find_Members("window.navigate"); 
commands.Add(Find_Members("window.open"));
commands.Add(Find_Members("document.open")); 
commands.Add(Find_Members("location.replace"));
	
result = All.GetParameters(commands);

result.Add(Find_JQuery_Outputs_Redirection());
result.Add(Find_MsAjax_Outputs_Redirection());