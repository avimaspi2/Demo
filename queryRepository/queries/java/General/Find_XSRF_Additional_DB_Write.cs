if (param.Length == 1)
{
	CxList db = param[0] as CxList;
	result = db.FindByShortNames(new List<string> {"update*", "delete*","insert*"});
}