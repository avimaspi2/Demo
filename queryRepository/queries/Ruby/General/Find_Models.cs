CxList baseClass = All.FindByShortName("Base").FindByType(typeof(TypeRef));


CxList classDecl = All.NewCxList();
foreach(CxList a in baseClass)
{
	
	TypeRef t = a.data.GetByIndex(0) as TypeRef;
	string type = t.TypeName;
	if(type.Equals("ActiveRecord.Base"))
	{
		
		classDecl.Add(a.GetFathers().GetFathers().FindByType(typeof(ClassDecl)));
	}
	
	
}
result = classDecl;