/// <summary>
/// Finds upload methods from PhoneGap API
/// </summary>
CxList methods = Find_Methods();

// relevant upload method
CxList reads = methods.FindByShortName("upload");
// from FileTranfer API
result.Add(reads.GetTargetOfMembers().FindByType("FileTransfer")
	.GetMembersOfTarget());