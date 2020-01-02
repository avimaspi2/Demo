/// <summary>
/// Find file input sanitizers (PHP)
/// </summary>

CxList methods = Find_Methods();

result = Find_Integers();
result.Add(methods.FindByShortNames(new List<string>(){ "round", "floor", "doubleval"}, false));	
result.Add(methods.FindByShortNames(new List<string>(){ "strlen", "stripslashes", "realpath", "basename" }, false));