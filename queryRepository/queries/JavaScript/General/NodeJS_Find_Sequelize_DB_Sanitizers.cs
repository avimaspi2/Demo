/**
Get all the sanitizers of the Sequelize ORM
**/

CxList sanitizers = All.NewCxList();

//All declarators
CxList decl = Find_Declarators();

//All db in invokes of sequelize
sanitizers = NodeJS_Find_Sequelize_DB_In();
//Get the query method
CxList dbquery = sanitizers.FindByShortName("query");

//Remove dbIn's that are not sanitizers
sanitizers -= sanitizers.FindByShortName("Sequelize");
//Remove potential vul methods
sanitizers -= dbquery;

//Replacements usage of method query 
CxList replacements = decl.FindByShortName("replacements");
//Binds usage of method query 
CxList bind = decl.FindByShortName("bind");

replacements.Add(bind);
CxList queryFlows = dbquery.InfluencedBy(replacements);

//query methods that are sanitized
result = queryFlows.GetStartAndEndNodes(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.GetStartEndNodesType.EndNodesOnly);
result.Add(sanitizers);