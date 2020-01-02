// This query finds the Database accesses where credentials 
// are stored on server and use a weak encryption
CxList weakCrypt = Find_Encode_and_Decode();
CxList reg = NodeJS_Find_Windows_Registry_Read();
CxList unknownRef = Find_UnknownReference();
CxList unkRefMembers = unknownRef.GetRightmostMember();
unknownRef.Add(unkRefMembers);

CxList resource = reg.GetMembersOfTarget();
resource.Add(reg);
resource.Add(NodeJS_Find_Read());
resource.Add(NodeJS_Find_DB_Out());

CxList resourceRef = All.DataInfluencedBy(resource).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
resource.Add(All.FindAllReferences(resourceRef) * weakCrypt.GetTargetOfMembers());

CxList pass = Find_Passwords();
CxList db = NodeJS_Find_DB_Base();

CxList candidates = resource.DataInfluencingOn(weakCrypt).DataInfluencingOn(pass);
candidates.Add(unknownRef.DataInfluencingOn(pass));

result = candidates.DataInfluencingOn(db, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);