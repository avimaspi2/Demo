string[] widgetTypes = param as string[];

if(widgetTypes != null)
{
	CxList konyAll = Kony_All();
	CxList labels = (Find_ObjectCreations() * konyAll).FindByTypes(widgetTypes);
	CxList labels_decl = labels.GetAssignee(konyAll);
	result = labels_decl;
}