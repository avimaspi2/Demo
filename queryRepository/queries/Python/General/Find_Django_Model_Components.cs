if (Find_Django().Count != 0)
{
	//Django Modules		
	CxList djangoModels = Find_Models_By_Import("django.db", "Model");
	djangoModels = djangoModels.FindByType(typeof(ClassDecl));

	foreach(CxList tRef in djangoModels){
		try{
			ClassDecl t = tRef.TryGetCSharpGraph<ClassDecl>();
			if(t != null && t.ShortName != null)		
			{				
				CxList access = All.FindByMemberAccess(t.ShortName + ".objects")
					.GetMembersOfTarget();
				result.Add(access);
			}
		}catch(Exception e)
		{
			cxLog.WriteDebugMessage(e.ToString());
		}
	}
}
else
{
	result = All.NewCxList();
}