// Find BroadCastReceiver classes that are exported
CxList manifest = Find_Android_Settings();
CxList broadCastRecveiers = manifest.FindByMemberAccess("RECIEVER.ANDROID_NAME", false).GetAssigner();
CxList receivers = broadCastRecveiers.GetAncOfType(typeof(IfStmt)); // find receiver tags

CxList recieverTree = manifest.GetByAncs(receivers);
CxList broadCasterReceiverExported = recieverTree.FindByMemberAccess("RECIEVER.ANDROID_EXPORTED").GetAssigner();
CxList exportedTrue = broadCasterReceiverExported.FindByShortName("true", false); // explicitly exported
CxList exportedBroadCast = exportedTrue.GetAncOfType(typeof(IfStmt));

// receiver with intent-filter tag is implicitly exported
CxList intentFilter = recieverTree.FindByMemberAccess("RECIEVER.INTENT_FILTER", false);
CxList intentFilterAncs = All.NewCxList();
intentFilterAncs.Add(intentFilter);
while (intentFilterAncs.Count > 0)
{
	intentFilterAncs = intentFilterAncs.GetFathers();
	exportedBroadCast.Add(intentFilterAncs * receivers);
}

// remove receivers that are explicitly not exported
CxList exportedFalse = broadCasterReceiverExported.FindByShortName("false", false);
CxList excluded = exportedFalse.GetAncOfType(typeof(IfStmt));
exportedBroadCast -= excluded;

CxList include = broadCastRecveiers.GetByAncs(exportedBroadCast);

if (include.Count > 0)
{
	// Convert the class names to an array
	char[] trims = new char[] {'\'', '"',  '.'};
	string[] types = new string[include.Count];
	int index = 0;
	foreach (CxList item in include)
	{
		string name = item.GetName();
		types[index] = name.Trim(trims);
		index++;
	}

	result = Find_Class_Decl().FindByTypes(types);
}