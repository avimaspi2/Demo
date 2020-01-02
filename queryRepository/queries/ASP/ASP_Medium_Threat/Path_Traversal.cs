CxList Inputs = Find_Interactive_Inputs();

CxList files = Find_Member_With_Target("Scripting.FileSystemObject", "OpenTextFile") +
	Find_Member_With_Target("Scripting.FileSystemObject", "CreateTextFile") +
	Find_Member_With_Target("Scripting.FileSystemObject", "GetFile") +
	Find_Member_With_Target("Scripting.FileSystemObject", "GetFolder");

CxList sanitized = Find_Integers();

CxList methods = Find_Methods();
methods = methods.FindByShortName("Replace",false);


foreach (CxList method in methods)
{
	CxList parameter = All.GetParameters(method,1);
	if (parameter == null || parameter.Count == 0)
	{
		continue;	
	}
	CSharpGraph mthodParam = parameter.data.GetByIndex(0) as CSharpGraph;
	string name = mthodParam.ShortName;
	if (String.IsNullOrEmpty(name))
	{
		continue;
	}
	//for the Replace methods that replaces ../ ./ ..\ .\ / \ etc
	if (name.Equals(@"""""../""""") ||name.Equals(@"""""./""""") ||name.Equals(@"""""..\""""")
		|| name.Equals(@""""".\""""")|| name.Equals(@"""""\""""") || name.Equals(@"""""/"""""))
	{
		sanitized.Add(method);
		
	}
}

result = files.InfluencedByAndNotSanitized(Inputs, sanitized);