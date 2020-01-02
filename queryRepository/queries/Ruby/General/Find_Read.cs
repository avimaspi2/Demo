CxList methods = Find_Methods();
result = methods.FindByShortName("file") + 
	methods.FindByShortName("fread") +
	methods.FindByShortName("fgets") +
	methods.FindByShortName("file_get_contents");

result.Add(
	All.FindByMemberAccess("File.read") +
	methods.FindByShortName("load_file"));