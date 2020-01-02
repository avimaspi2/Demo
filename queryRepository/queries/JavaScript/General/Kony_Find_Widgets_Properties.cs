string[] widgetTypes = param as string[];

if(widgetTypes != null)
{
	CxList labels_decl = Kony_Find_Widgets_Declarations(widgetTypes);
	CxList jsMemberAccesses = (Find_MemberAccesses() * Kony_All()).FindByFileName("*.json");
	
	result = jsMemberAccesses.FindAllReferences(labels_decl).GetMembersOfTarget();
}