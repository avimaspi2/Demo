CxList triggersCode = Find_Triggers_Code();

CxList triggers = triggersCode.FindByType(typeof(MethodDecl)).FindByRegex("trigger ").FindByRegex(" on ");
CxList attrs = triggersCode.FindByFathers(triggers);

System.Collections.ArrayList triggersOn = new System.Collections.ArrayList();

foreach (CxList cml in attrs)
{
	if (cml.data.Count > 0)
	{
		CustomAttribute atr = cml.data.GetByIndex(0) as CustomAttribute;
		if (atr != null && !atr.Name.Equals(""))
		{
			if (!triggersOn.Contains(atr.Name))
			{
				triggersOn.Add(atr.Name);
			}
			else
			{
				result.Add(attrs.FindByCustomAttribute(atr.Name));
			}
		}
	}
}

result -= Find_Test_Code();