if(Find_Ctp_Files().Count > 0)
{
	CxList controller = All.FindByType(typeof(ClassDecl)).FindByShortName("*sController");

	CxList thisRef = All.FindByType(typeof(ThisRef));
	CxList modelCall = thisRef.GetMembersOfTarget().FindByType(typeof(MemberAccess));
	List < string > addStrings = new List<string> {"save*", "set", "updateAll", "delete*"};
	foreach(CxList control in controller)
	{
		ClassDecl cd = control.data.GetByIndex(0) as ClassDecl;
		if(cd != null)
		{
			String s = cd.ShortName;
			int index = s.IndexOf("sController");
			s = s.Remove(index);
			CxList curModel = modelCall.FindByShortName(s);		
			CxList model = curModel.GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr));
			result.Add(model.FindByShortNames(addStrings));
		}
	} 
	//Cases like appModel -> query()
	result.Add(Find_Cake_DB_In_Query());
}