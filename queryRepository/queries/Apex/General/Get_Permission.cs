/*
(1) Account.fields.Name.getDescribe().isUpdateable()
(2) Account.fields.getMap().get('Name').getDescribe().isUpdateable()

(3) Schema.getGlobalDescribe.get('Account').getDescribe().fields.Name.isUpdateable()
(4) Schema.getGlobalDescribe.get('Account').getDescribe().fields.getMap().get('Name').getDescribe().isUpdateable()

(5) Schema.sObjectType.Account.Name.isUpdateable()
(6) Schema.sObjectType.Account.getMap().get('Name').getDescribe().isUpdateable()
(7) Schema.sObjectType.Account.fields.Name.isUpdateable()
(8) Schema.sObjectType.Account.getMap().fields.get('Name').getDescribe().isUpdateable()
*/

int start = Environment.TickCount / 100;
if ((param.Length == 5) || (param.Length == 3))
{
	string permission = param[0] as string;
	string obj = param[1] as string; 
	string obj1 = "\"'" + obj + "'\"";
	string field = param[2] as string; 
	string field1 = "\"'" + field + "'\"";
	
	CxList fieldList = All.NewCxList();
	CxList objList = All.NewCxList();
	if (param.Length == 5)
	{
		fieldList = param[3] as CxList;
		objList = param[4] as CxList;
	}
	else
	{
		objList = Find_Apex_Files().FindByShortName(obj, false);
		fieldList = Find_Apex_Files().FindByShortName(field, false);
	}
	
	CxList strings = Find_Strings();
	CxList getParams = Find_Parameters_of_get();
	CxList getVarParams = getParams - getParams.FindByShortName("\"'*") - getParams.FindByShortName("'*");
	CxList objList1 = getVarParams + getParams.FindByShortName(obj1, false);
	CxList fieldList1 = getVarParams + getParams.FindByShortName(field1, false);

	CxList allPer = Find_General_Permissions().FindByShortName(permission, false);

	foreach (CxList per in allPer)
	{
		if ((per //1
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(field, false)
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(obj, false).Count > 0)
			||
			(per //2
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByParameters(fieldList1)
			.GetTargetOfMembers()
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(obj, false).Count > 0)
			||
			(per //3
			.GetTargetOfMembers().FindByShortName(field, false)
			.GetTargetOfMembers()
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByParameters(objList1).Count > 0)
			||
			(per //4
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByParameters(fieldList1)
			.GetTargetOfMembers()
			.GetTargetOfMembers()
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByParameters(objList1).Count > 0)
			||
			(per //5
			.GetTargetOfMembers().FindByShortName(field, false)
			.GetTargetOfMembers().FindByShortName(obj, false).Count > 0)
			||
			(per //6
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(field)
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(obj, false).Count > 0)
			||
			(per //7
			.GetTargetOfMembers().FindByShortName(field, false)
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(obj, false).Count > 0)
			||
			(per //8
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(field, false)
			.GetTargetOfMembers()
			.GetTargetOfMembers()
			.GetTargetOfMembers().FindByShortName(obj, false)
			.Count > 0)
			)
		{
			result.Add(per);
		}
	}
}