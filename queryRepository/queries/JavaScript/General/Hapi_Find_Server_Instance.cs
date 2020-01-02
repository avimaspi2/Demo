CxList methods = Find_Methods();
CxList leftsideAssign = Find_Assign_Lefts();
CxList references = All.NewCxList();
CxList unknownReferences = Find_UnknownReference();
CxList declarators = Find_Declarators();
CxList methodDecls = Find_MethodDecls();
CxList imports = Find_Import();

references.Add(methods, declarators, methodDecls, unknownReferences);

List<string> methodsCreateHapiServer = new List<string>{"server","Server","createServer"};

CxList serverObjCreation = All.NewCxList();
foreach(CxList imp in imports){
	Import import = imp.TryGetCSharpGraph<Import>();
	if(import.ImportedFilename.Equals("hapi")){
		if(import.NamespaceAlias != null){
			CxList aliasVars = unknownReferences.FindByShortName(import.NamespaceAlias);
			if (import.SymbolAliases.Count > 0)
			{
				// For case: var server = require('hapi').Server
				CxList serverMember = aliasVars.GetMembersOfTarget().FindByShortNames(methodsCreateHapiServer);
				CxList serverRefs = unknownReferences.FindAllReferences(serverMember.GetAssignee());
				serverObjCreation.Add(serverRefs.GetAncOfType(typeof(ObjectCreateExpr)));
			}
			else
			{
				// For case: var Hapi = require('hapi'); var server = new Hapi.Server();
				CxList serverRefs = unknownReferences.FindAllReferences(aliasVars.GetAssignee());
				CxList serverMember = serverRefs.GetMembersOfTarget().FindByShortNames(methodsCreateHapiServer);
				serverObjCreation.Add(serverMember.GetAncOfType(typeof(ObjectCreateExpr)));
			}
		}
	}
}
result.Add(serverObjCreation.GetAssignee());

CxList hapiRequires = Find_Require("hapi") - imports;

CxList serverCreationTargets = methods.FindByShortNames(methodsCreateHapiServer).GetTargetOfMembers();

CxList hapiTargets = serverCreationTargets.DataInfluencedBy(hapiRequires);
hapiTargets.Add(serverCreationTargets * hapiRequires);

//Workaround for missing variable declaration flow
CxList serverCreationTargetReferences = All.FindAllReferences(serverCreationTargets) - serverCreationTargets;
CxList varsInfluencedByHapiRequires = serverCreationTargetReferences.DataInfluencedBy(hapiRequires)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
hapiTargets.Add(serverCreationTargets.FindAllReferences(varsInfluencedByHapiRequires));

result.Add(unknownReferences.FindAllReferences(leftsideAssign.DataInfluencedBy(hapiTargets)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly)));

//Add exported server references to result list
CxList moduleExports = leftsideAssign.FindByShortName("cxExports");
CxList exportedServers = result.GetAssignee() * moduleExports;
CxList importedServerRef = exportedServers.DataInfluencingOn(leftsideAssign)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
result.Add(unknownReferences.FindAllReferences(importedServerRef));