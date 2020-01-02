CxList methods = All.FindByShortName("digest") + All.FindByShortName("hexdigest");
CxList digestSHAMD = All.FindByMemberAccess("Digest.MD5") + All.FindByMemberAccess("Digest.SHA1") +
	All.FindByMemberAccess("HMAC.MD5") + All.FindByMemberAccess("HMAC.SHA1");
CxList action = digestSHAMD.GetMembersOfTarget();

result.Add(action.FindByShortName("file").GetMembersOfTarget() * methods);
result.Add(action * methods);

digestSHAMD = digestSHAMD.GetMembersOfTarget().FindByShortName("new");
CxList oce = All.FindByType(typeof(ObjectCreateExpr)) * (All.FindByShortName("MD5") + All.FindByShortName("SHA1") + All.FindByShortName("Digest"));
digestSHAMD.Add(oce);

CxList ur = All.FindByType(typeof(UnknownReference));
CxList baseAssign = digestSHAMD.GetAncOfType(typeof(AssignExpr));

CxList refToDigest = ur.GetByAncs(baseAssign).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList curRef = ur.FindAllReferences(refToDigest);
result.Add(curRef.GetMembersOfTarget() * methods);
/*
OpenSSL::HMAC.digest(digest, "superscret", "Lorem ipsum dolor sit amet")
OpenSSL::HMAC.hexdigest(digest, "superscret", "Lorem ipsum dolor sit amet")
*/

result+= ur.GetParameters(methods.FindByMemberAccess("HMAC.*"),0);


/*
# one-liner example
puts Digest::HMAC.hexdigest("data", "hash key", Digest::SHA1)

# rather longer one
hmac = Digest::HMAC.new("foo", Digest::RMD160)

This usage is discouraged
*/

CxList hmacMA = All.FindByMemberAccess("DIGEST.HMAC");
CxList digest = All.GetParameters(hmacMA.GetMembersOfTarget() * methods);
digest.Add(All.GetParameters(All.FindByShortName("HMAC").FindByType(typeof(ObjectCreateExpr))));
digest=digest.FindByMemberAccess("Digest.SHA1") + digest.FindByMemberAccess("Digest.MD5");
result.Add(digest.FindByShortName("SHA1") + digest.FindByShortName("MD5"));