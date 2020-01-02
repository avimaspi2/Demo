// Find passwords but exclude some by heuristic, for example exclude variables that are all uppercase

CxList passwords = Find_All_Passwords();

// 1)exclude variables that are all uppercase - usually describes the pattern of the data, such as PASSWORDPATTERN, PASSORDTYPE...
CxList upperCase = All.NewCxList();
foreach (CxList res in passwords)
{
	string name = res.GetName();
	if (name.ToUpper().Equals(name))
	{
		upperCase.Add(res);
	}
}

passwords -= upperCase;

result = passwords;