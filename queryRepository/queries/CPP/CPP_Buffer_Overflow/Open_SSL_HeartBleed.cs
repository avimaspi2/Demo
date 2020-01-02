////////////////////////////////////////////////////////////////////////////////////////////
// Query  : Open_SSL_HeartBleed
// Purpose: Find the HeartBleed vulnerability in Open SSL.
//

CxList methods = Find_Methods();
CxList n2s = All.FindByShortName("n2s");
CxList payload = All.GetParameters(n2s, 1);
CxList memcpy = All.GetParameters(methods.FindByShortName("memcpy"), 2);
CxList returns = Find_ReturnStmt();
CxList ifs = Find_Ifs();
CxList payloadAncs = All.GetByAncs(payload);
CxList payloadAllRef = All.FindAllReferences(payloadAncs);
CxList conditions = Find_Conditions();
CxList safe = All.NewCxList();

foreach(CxList currentPayload in payload)
{
	// find payload in an "if" condition that causes return
	CxList p1 = payloadAncs.GetByAncs(currentPayload) - currentPayload;
	CxList payloadRef = payloadAllRef.FindAllReferences(p1);
	
	foreach(CxList currentRef in payloadRef)
	{
		CxList payloadUnderIf = currentRef.GetByAncs(conditions).GetAncOfType(typeof(IfStmt));
		
		if (returns.GetByAncs(payloadUnderIf).Count > 0)
		{
			// if the payload is under if that causes return then the payload is safe
			safe.Add(currentPayload);
			safe.Add(p1);
		}
	}
}

// subtract safe 
payload -= safe;
result = memcpy.InfluencedBy(payload);