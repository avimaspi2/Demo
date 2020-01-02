/// <summary>
/// Return all objects in django production configuration files.
/// </summary>
if (Find_Django().Count != 0)
{
	string[] configFileNames = new string [] {"production.py", "settings.py"};
	foreach (string fileName in configFileNames)
	{
		result.Add(All.FindByFileName("*" + fileName));
	}
}
else
{
	result = All.NewCxList();
}