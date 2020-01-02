if (param.Length == 1)
{
	string fileName = (string)param[0];
	CxList allExprInProject = Lightning_Find_All_Expressions_In_Project();
	result = allExprInProject.FindByFileName(fileName);
}