/// <summary>
/// Finds the File Download API
/// </summary>
CxList methods = Find_Methods();

CxList reads = methods.FindByShortName("download");

// check if the method belongs to a FileTransfer Object
result.Add(reads.GetTargetOfMembers().FindByType("FileTransfer")
	.GetMembersOfTarget());