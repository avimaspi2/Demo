CxList methodInvoke = Find_Methods();

result = methodInvoke.FindByShortNames(
	new List<string> {
		"toBoolean",
		"toDate",
		"toFloat",
		"toInt",
		"byteLength"
		}
	);
result.Add(All.FindByMemberAccess("*.length"));