CxList methodInvoke = Find_Methods();
CxList objCreates = Find_ObjectCreations();
CxList inLeftOfAssign = Find_Assign_Lefts();
CxList output = All.NewCxList();

CxList referencesOfFS = Find_Require("fs");
	
CxList allInfluByRequireFS = methodInvoke.DataInfluencedBy(referencesOfFS);

CxList createWStream = allInfluByRequireFS.FindByShortName("createWriteStream");
CxList leftOfFSCreatStream = inLeftOfAssign.GetByAncs(createWStream.GetAncOfType(typeof(AssignExpr)));
CxList allCreateStreamObj = All.FindAllReferences(leftOfFSCreatStream);
// Add all methods of createWriteStream object
allInfluByRequireFS.Add(methodInvoke.DataInfluencedBy(allCreateStreamObj));
// All methodInvoke influenced by required liblrary
CxList methInvFromRequireFS = allInfluByRequireFS * methodInvoke;
// Add fs.SyncWriteStream (despite deprecated can appear in legacy code)
methInvFromRequireFS.Add(objCreates.FindByShortName("SyncWriteStream"));

////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	1. Find fs.write methods
*/
CxList fsWrite = methInvFromRequireFS.FindByShortNames(
	new List<string> {
		"write",
		"writeFile",
		"readFileSync",
		"appendFile",
		"appendFileSync",
		"copyFile",
		"copyFileSync"
		}
	);

output.Add(fsWrite);

//**********************************************************************************

result = output - XS_Find_All();