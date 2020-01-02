/*
This query finds incorrect implementations of authentication algorithm.
For example: A direct comparison of user input and a hardcoded string

if ($_GET['admin'] == 'login'){
	...
}

*/

CxList strings = Find_Strings();
CxList user_parameter = strings.FindByShortNames(new List<String>(){ "*admin*", "*user*" });
CxList variables = user_parameter.GetAncOfType(typeof(IndexerRef));

CxList user_input = variables.FindByShortNames(new List<String>(){ "_POST", "_GET", "_REQUEST", "_COOKIE" }) - 
	Find_Left_Side_Sanitize();

CxList conditions = Find_Conditions().FindByType(typeof(BinaryExpr));
conditions *= strings.GetFathers().FindByType(typeof(BinaryExpr));

result = user_input.FindByFathers(conditions);