CxList methods = Find_Methods();

// java.util.Date
CxList temp = methods.FindByMemberAccess("Date.*");
result.Add(temp.FindByShortNames(new List<string>{
		"getDate",
		"getDay",
		"getHours",
		"getMinutes",
		"getMonth",
		"getSeconds",
		"getTimezoneOffset",
		"getYear",
		"parse",
		"setDate",
		"setHours",
		"setMinutes",
		"setMonth",
		"setSeconds",
		"setYear",
		"toGMTString",
		"toLocaleString",
		"UTC"}));

// java.util.Properties
result.Add(methods.FindByMemberAccess("Properties.save"));