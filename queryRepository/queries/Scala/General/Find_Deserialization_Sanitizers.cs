result = All.FindByMemberAccess("MessageDigest.digest");
result.Add(All.FindByMemberAccess("MessageDigest.update"));

// XStream Deserialization Library sanitization
CxList methods = Find_Methods();
CxList uRefs = Find_UnknownReference();
CxList XStreamReaders = Find_XStream_Deserialization_Methods();

//It's safe to remove all permissions from the Whitelist, using "xs.addPermission(NoTypePermission.NONE);"
CxList xStreamAddPermissions = methods.FindByMemberAccess("XStream.addPermission");
CxList safeAddParameters = uRefs.FindByShortName("NoTypePermission").GetMembersOfTarget().FindByShortName("NONE");
CxList XStreamSanitizers = xStreamAddPermissions.FindByParameters(safeAddParameters);

//It's safe to add all permissions to the Blacklist, using "xs.denyPermission(AnyTypePermission.ANY);"
CxList xStreamDenyPermissions = methods.FindByMemberAccess("XStream.denyPermission");
CxList safeDenyParameters = uRefs.FindByShortName("AnyTypePermission").GetMembersOfTarget().FindByShortName("ANY");
XStreamSanitizers.Add(xStreamDenyPermissions.FindByParameters(safeDenyParameters));

//It's unsafe to add all permissions to Whitelist or remove all permissions from the Blacklist.
CxList unsafePermissions = xStreamDenyPermissions.FindByParameters(safeAddParameters);
unsafePermissions.Add(xStreamAddPermissions.FindByParameters(safeDenyParameters));

CxList sanitizedXStreamObjects = XStreamSanitizers.GetTargetOfMembers();
CxList vulnerableXStreamObjects = unsafePermissions.GetTargetOfMembers();
CxList sanitizedXStreamReaders = XStreamReaders.InfluencedByAndNotSanitized(sanitizedXStreamObjects, vulnerableXStreamObjects);
sanitizedXStreamReaders = sanitizedXStreamReaders.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);