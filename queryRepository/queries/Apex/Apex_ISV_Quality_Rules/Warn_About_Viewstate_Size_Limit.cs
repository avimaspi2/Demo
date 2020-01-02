//Warn if any non-transient variable is found in a controller or extension
CxList nonTransientFields = Find_Apex_Files().FindByType(typeof(FieldDecl));
nonTransientFields -= nonTransientFields.FindByFieldAttributes(Modifiers.Transient)
	+ nonTransientFields.FindByFieldAttributes(Modifiers.Static);
CxList nonTransientClasses = nonTransientFields.GetAncOfType(typeof(ClassDecl));

CxList extensionNames = Find_VF_Pages().FindByType(typeof(TypeRef));
nonTransientClasses = nonTransientClasses.FindByShortName(extensionNames);

foreach (CxList cls in nonTransientClasses) 
{	//Find the pages that have cls as a controller or extension
	CxList refs = extensionNames.FindByShortName(cls);
	CxList nonTransientFieldsInCls = nonTransientFields.GetByAncs(cls);
	foreach (CxList field in nonTransientFieldsInCls) 
	{	//Output each non-transient in cls, linked to the pages found above
		foreach (CxList curRef in refs) 
		{
			result.Add(field.Concatenate(curRef, true));
			break;
		}
	}
}