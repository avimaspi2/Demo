CxList Inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList Logon = methods.FindByShortName("LogonUser");

CxList DuplicateToken = methods.FindByShortName("DuplicateToken");

CxList Impersonate = All.FindByMemberAccess("WindowsIdentity.Impersonate");
Impersonate.Add(All.FindByShortName("ImpersonateLoggedOnUser"));

//only parameters of logonuser methods that are influenced by inputs
CxList LogonParams = All.GetParameters(Logon);		//all parameters of LogonUser method
CxList LogonParamsIn = LogonParams.DataInfluencedBy(Inputs);	//parameters of LogonUser method that influenced by input
LogonParamsIn = LogonParams.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

//only parameters of duplicatetoken methods that are influenced by logonuser methods parameters
CxList DuplicateTokenParams = All.GetParameters(DuplicateToken);	//all parameters of DuplicateToken method	
CxList DuplicateTokenParamsLP = DuplicateTokenParams.DataInfluencedBy(LogonParams); //parameters of DuplicateToken method that influenced by input
DuplicateTokenParams = DuplicateTokenParams.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList prms = All.NewCxList();

foreach(CxList curLogon in Logon)
{	
	CxList prm = LogonParams.GetParameters(curLogon);
	CxList prmIn = LogonParamsIn.GetParameters(curLogon);

	if(prmIn.Count > 0)
	{
		prms.Add(prm);
		
		foreach(CxList curDuplicate in DuplicateToken)
		{	
			CxList prmDupl = DuplicateTokenParams.GetParameters(curDuplicate);
			CxList prmDupLP = DuplicateTokenParamsLP.GetParameters(curDuplicate);
			if(prmDupLP.Count > 0)
			{
				prms.Add(prmDupl);
			}
		}
	}
}
result.Add(Impersonate.DataInfluencedBy(prms));
result.Add(All.FindByShortName("CreateProcessWithLogonW").DataInfluencedBy(Inputs));