CxList unknRefs = Find_Unknown_References();
CxList inputs = Find_Inputs();

CxList nodes = Find_Improper_Index_Access(false);

//Creates flow with indexerRefs that are influenced by user input
CxList unkRefsIndexer = unknRefs.FindByFathers(nodes);
CxList inputsNodes = unkRefsIndexer.DataInfluencedBy(inputs);
CxList inputNodesUnkRefs = inputsNodes.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList inputNodeIndexerRef = inputNodesUnkRefs.GetAncOfType(typeof(IndexerRef));
nodes -= inputNodeIndexerRef;
nodes.Add(inputsNodes);
result = nodes;