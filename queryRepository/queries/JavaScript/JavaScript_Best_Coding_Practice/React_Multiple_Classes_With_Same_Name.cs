CxList reactClasses = React_Find_ClassComponent();

List<string> classNames = new List<string>();
foreach(CxList cls in reactClasses){
	string className = cls.GetName();
	if (classNames.Contains(className))
	{
		result.Add(cls);
	}
	classNames.Add(className);
}