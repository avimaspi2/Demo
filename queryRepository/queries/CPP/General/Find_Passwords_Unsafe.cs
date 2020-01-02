// Find passwords in code which might be sensitive

CxList passwords = Find_All_Passwords();

// Exclude variables that are all uppercase - usually describes the pattern of the data, such as PASSWORDPATTERN, PASSORDTYPE...
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