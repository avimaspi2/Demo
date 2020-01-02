/* Auxiliary information */
CxList unkRefs = Find_UnknownReference();
CxList declarators = Find_Declarators();

/* Determine if there are Express apps not using csurf/csrf. */
CxList unsecureExpressApps = All.NewCxList();

/* Finds statements of the form (where «x» is arbitrary):
	var x = require('express');
and factors them as:
	requireExpressLHS = x
	requireExpressRHS = require('express');
*/

CxList requireExpressLHS = Find_Require("express");

/* Finds statements of the form (where «y» is arbitrary and «x» is in requireExpressLHS):
	var y = ...
	y = x();
or simply
	var y = x();
and factors them as:
	expressAppInitLHS = y
	expressAppInitRHS = x();
	expressAppDefs = var y;

*/

CxList expressAppDefs = declarators.FindDefinition(requireExpressLHS.GetAssignee());

/* Finds all occurrences of «y» in the source-code, for «y» in expressAppDefs. */
CxList expressAppUnkRef = unkRefs.FindAllReferences(expressAppDefs);

/* Finds all occurrences of methods named .csrf() targetting objects returned by
	require('express') or by require('csurf') invocations. */
CxList expressAndCsurf = Find_Require("express", 2);
expressAndCsurf.Add(Find_Require("csurf", 2));

CxList csrfMethodInv = expressAndCsurf.GetMembersOfTarget().FindByShortName("csrf");

/* Finds all occurrences of x.use(...), for «x» in expressAppUnkRef. */
CxList expressAppUses = expressAppUnkRef.GetMembersOfTarget().FindByShortName("use").FindByType(typeof(MethodInvokeExpr));

/* Filters the set of «x.use(...)» occurrences by those that are data-influenced by objects in csrfMethodInv. Later
returns the definitions of those «x».s */
CxList safeExpressAppUses = expressAppUses.DataInfluencedBy(csrfMethodInv)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList safeExpressAppDefs = expressAppDefs.FindDefinition(safeExpressAppUses.GetTargetOfMembers());

unsecureExpressApps.Add(expressAppDefs - safeExpressAppDefs);

/* If there are unsecure Express apps, look for database updates influenced by non-sanitized inputs. */
if (unsecureExpressApps.Count > 0)
{
	CxList updateDB = All.FindByParameters(NodeJS_Find_DB_IN());
	string[] findMethodsNames = {"findAndModify", "insert", "update*", "drop", "remove", "renameCollection",
		"save", "delete*", "copy", "destroy", "del", "findByIdAnd*", "upsert", "bulkCreate", "bulkWrite", "insertOrUpdate"};
	updateDB = updateDB.FindByShortNames(new List<string>(findMethodsNames));
	CxList sanitizers = NodeJS_Find_General_Sanitize();
	CxList inputs = NodeJS_Find_Interactive_Inputs();
	result = updateDB.InfluencedByAndNotSanitized(inputs, sanitizers);
}