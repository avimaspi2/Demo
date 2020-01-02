if(param.Length == 1)
{
	CxList sanitize = param[0] as CxList;
	result = sanitize.FindByShortNames(new List<string> {
			"size",
			"length"});
}