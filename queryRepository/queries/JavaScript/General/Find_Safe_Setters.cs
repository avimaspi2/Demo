CxList setter = Find_Set_Attribute_Structure();
CxList events = Find_Vulnerable_SetAttribute();
setter -= events;
result.Add(setter);
CxList methods = Find_Methods();

CxList onEvents = Find_OnEvents();
CxList target = setter.GetTargetOfMembers();
CxList allRefs = All.FindByShortName(target).FindAllReferences(target);
CxList createAttr = methods.FindByShortName("createAttribute", false);
CxList caparam = All.GetParameters(createAttr);
CxList vulnerableParam = caparam - onEvents;

//handle createAttribute
foreach(CxList setr in setter)
{
	CxList member = setr.GetTargetOfMembers();
	CxList isThisRef = member.FindByType(typeof(ThisRef));
	if (isThisRef.Count == 0)
	{
		CxList temp = allRefs.FindAllReferences(member);
		CxList t = createAttr.GetByAncs(temp.GetFathers());
		CxList vul = vulnerableParam.GetParameters(t);
		if(vul.Count > 0)
		{
			result.Add(setr);
		}
	}
}