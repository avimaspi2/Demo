/**
	This query returns all passwords usages being stored with bad encrypt algorithms.
	It will return flows from the password to its storage.	
**/

// Finds passwords and storing actions
CxList passwords = Find_Passwords();
CxList weakEncrypters = Find_Weak_Encryption();
CxList outputs = All.NewCxList();
outputs.Add(Find_DB_In());
outputs.Add(Find_Write());
outputs.Add(Find_Log_Outputs());
outputs.Add(Find_Outputs());

// Creating flows from passwords to weak encryptions
CxList passToWeak = Get_Composed_Path(passwords.InfluencingOnAndNotSanitized(weakEncrypters, Find_Sanitize()), weakEncrypters);

// Creating flows from the weak encryptions to the storage action
CxList passToWeakEndNodes = passToWeak.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList passToOuts = Get_Composed_Path(passToWeak, passToWeakEndNodes.InfluencingOn(outputs));

// Returning the flows from password to storage
result.Add(passToOuts.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));