CxList inputs = Find_Interactive_Inputs();
inputs.Add(Find_Read());
inputs.Add(Find_DB());

CxList format = Get_Format_Parameter();
CxList sanitize = Find_Integers();

result = inputs.InfluencingOnAndNotSanitized(format, sanitize);

// Now find all cases in which the number of expected args is bigger then the number of actual args
var StringLiterals = Find_Strings();
format = format * StringLiterals;
foreach(CxList formatString in format){
	var formatMethods = formatString.GetAncOfType(typeof(MethodInvokeExpr));
	var numOfParams = (All.GetParameters(formatMethods)).Count / 2;
	var formatedStringPosition = formatString.GetFathers().GetIndexOfParameter();
	var formatStringName = formatString.GetName();
	var percentagesInFormatString = Regex.Matches(formatStringName, "%").Count;
	percentagesInFormatString -= Regex.Matches(formatStringName, "%\\d+?\\$").Count; // Remove %num$
	while (formatStringName.Contains("%%")){ // Remove %%
		percentagesInFormatString -= 2;
		var rgx = new Regex("%%");
		formatStringName = rgx.Replace(formatStringName, "", 1);
	}
	
	if (percentagesInFormatString > numOfParams - 1 - formatedStringPosition)
		result.Add(formatString);
}