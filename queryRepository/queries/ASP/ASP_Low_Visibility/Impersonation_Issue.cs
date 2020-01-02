CxList Inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList Logon = methods.FindByShortName("logonuser");
CxList DuplicateToken = methods.FindByShortName("duplicatetoken");

CxList Impersonate = All.FindByMemberAccess("windowsidentity.impersonate") + 
					All.FindByShortName("impersonateloggedonuser");

foreach(CxList curLogon in Logon)
{	
	CxList prm = All.GetParameters(curLogon);
	if(prm.DataInfluencedBy(Inputs).Count > 0)
	{
		result.Add(Impersonate.DataInfluencedBy(prm));
		
		foreach(CxList curDuplicate in DuplicateToken)
		{	
			CxList prmDupl = All.GetParameters(curDuplicate);
			if(prmDupl.DataInfluencedBy(prm).Count > 0)
			{
				result.Add(Impersonate.DataInfluencedBy(prmDupl));
			}
		}
	}
}

result.Add(All.FindByShortName("createprocesswithlogonw").DataInfluencedBy(Inputs));