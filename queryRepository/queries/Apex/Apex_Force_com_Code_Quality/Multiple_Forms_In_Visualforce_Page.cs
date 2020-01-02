CxList formStart = Find_Methods().FindByShortName("__multiple_form__");

System.Collections.ArrayList formsFileNames = new System.Collections.ArrayList();

foreach (CxList form in formStart)
{
	if (form.data.Count > 0)
	{
		CSharpGraph g = form.data.GetByIndex(0) as CSharpGraph;
		if (g != null)
		{
			string fileName = g.LinePragma.FileName;
			if (!formsFileNames.Contains(fileName))
			{
				formsFileNames.Add(fileName);
			}
			else
			{
				result.Add(formStart.FindByFileName(fileName));
			}
		}
	}
}

result -= Find_Test_Code();