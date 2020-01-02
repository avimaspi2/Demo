// CERT DRD05-J. Do not grant URI permissions on implicit intents
// An implicit intent should not be granted FLAG_GRANT_READ_URI_PERMISSION or
// FLAG_GRANT_WRITE_URI_PERMISSION permissions.

CxList methods = Find_Methods();
CxList intent = methods.FindByMemberAccess("Intent.*", false);

CxList intentFlags = intent.FindByMemberAccess("Intent.setFlags");
intentFlags.Add(intent.FindByMemberAccess("Intent.addFlags"));

CxList flagsReadWrite =	All.FindByMemberAccess("Intent.FLAG_GRANT_READ_URI_PERMISSION");
flagsReadWrite.Add(All.FindByMemberAccess("Intent.FLAG_GRANT_WRITE_URI_PERMISSION"));

CxList addedReadWrite = flagsReadWrite.GetByAncs(intentFlags);

// The sanitizer is setting an explicit component / class.
// The vulnerability is only for an implicit intent.
CxList sanitize = intent.FindByMemberAccess("Intent.setComponent");
sanitize.Add(intent.FindByMemberAccess("Intent.setClass"));

sanitize = sanitize.GetTargetOfMembers();


CxList startActivity = methods.FindByShortNames(new List<string> {
		"startActivity",
		"startService",
		"startActivities"});

result = startActivity.InfluencedByAndNotSanitized(addedReadWrite, sanitize);