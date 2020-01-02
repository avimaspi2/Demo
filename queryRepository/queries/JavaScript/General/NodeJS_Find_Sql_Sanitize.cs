//https://thecodebarbarian.wordpress.com/2014/09/04/defending-against-query-selector-injection-attacks/

//1. Heuristic
CxList allMeth = Find_Methods();
allMeth.Add(Find_MethodRef());
result = allMeth.FindByShortName("sanitiz*");

//2. we are looking for this sanitation (weak, but something):
// var clean = (req.body.username || "").toString(10);
CxList bin = Find_Binarys();
//find all binarys with one son = ""
CxList sons = All.FindByFathers(bin).FindByShortName("");
//back to only this Binarys
bin = sons.GetFathers();
foreach(CxList gb in bin) // for each good Binary
{   // look if we have the ").toString()" after clousers.
	CxList members = gb.GetMembersOfTarget().FindByShortName("toString");
	if(members.Count > 0)
	{
		result.Add(gb);
	}
}
 
//3. Explicitly specify the query selector ($in or $eq) when querying with untrusted data:
// username: { $in: [req.body.username] } 
CxList unkref = Find_UnknownReference();
//as at exampl, input will pass true the assigment to $in or $eq
result.Add(unkref.FindByShortName("$in"));
result.Add(unkref.FindByShortName("$eq"));

// Find NodeJS Crypto library
CxList cryptoRequire = Find_Require("crypto");
CxList cryptoCreateHash = cryptoRequire.GetMembersOfTarget().FindByShortName("createHash");
CxList cryptoCreateHashUnkRefs = unkref.FindAllReferences(cryptoCreateHash.GetAssignee());
cryptoCreateHash.Add(cryptoCreateHashUnkRefs);

CxList parameters = Find_Param();
CxList digestWithParams = allMeth.FindByShortName("digest").FindByParameters(parameters);
result.Add(digestWithParams.InfluencedBy(cryptoCreateHash));