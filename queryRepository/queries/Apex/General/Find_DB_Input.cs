result = Find_DB_Inactive();

CxList vf = Find_VF_Pages();
CxList apex = Find_Apex_Files();

// Add the x__c.y__c syntax
CxList __c = vf.FindByShortName("*__c") + vf.FindByShortName("*__r");
__c = __c.GetMembersOfTarget();
__c = __c.FindByShortName("*__c") + __c.FindByShortName("*__r");
__c.Add(__c.GetMembersOfTarget());

// Add the same where x__c is replaced by any custom object
CxList cobj = Find_Custom_sObjects().FindByFathers(All.FindByType(typeof(ReturnStmt)));
CxList objMethods = apex.GetMethod(cobj);
CxList cobjGet = vf.FindAllReferences(objMethods);
cobjGet = cobjGet.GetMembersOfTarget();
cobjGet = cobjGet.FindByShortName("*__c") + cobjGet.FindByShortName("*__r") + __c;

// Leave only the rightmost part of the ariable
cobjGet.Add(cobjGet.GetMembersOfTarget());
cobjGet.Add(cobjGet.GetMembersOfTarget());
cobjGet -= cobjGet.GetTargetOfMembers();
result.Add(cobjGet);

// Add the sObj.* syntax
CxList sObj = vf * Find_Schema_sObjects();
sObj = sObj.GetMembersOfTarget();
sObj -= sObj.FindByShortName("*id");
result.Add(sObj);

// Check if there are multiple results in the exact same LinePragma and name, and remove the double results.
CxList vfResult = result * vf;
CxList resultsToRemove = All.NewCxList();
foreach(CxList obj1 in vfResult)
{
	CSharpGraph g1 = obj1.data.GetByIndex(0) as CSharpGraph;
	foreach(CxList obj2 in vfResult - obj1 - resultsToRemove)
	{
		CSharpGraph g2 = obj2.data.GetByIndex(0) as CSharpGraph;
		if (g1.ShortName == g2.ShortName &&
			g1 != g2 &&
			g1.LinePragma.FileName == g2.LinePragma.FileName &&
			g1.LinePragma.Line == g2.LinePragma.Line)
		{
			resultsToRemove.Add(obj1);
		}
	}
}
result -= resultsToRemove;

// Standard Controller (hidden queries)
result.Add(apex.FindByMemberAccess("StandardController.*"));
result.Add(apex.FindByMemberAccess("StandardSetController.*"));

// Custom settings
result.Add(apex.FindByMemberAccess("MyObject__c.getAll", false));
result.Add(apex.FindByMemberAccess("MyObject__c.getInstance", false));
result.Add(apex.FindByMemberAccess("MyObject__c.getValues", false));