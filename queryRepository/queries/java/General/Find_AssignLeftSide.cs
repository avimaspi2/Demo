// Custom attributes
CxList customAttributes = Find_UnknownReference().GetByAncs(Find_CustomAttribute());

CxList assign = Find_AssignExpr();

// Find left side of the assign expression
CxList assignLeftSide = All.GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Left) - customAttributes;
// Remove indexes of arrays
assignLeftSide = assignLeftSide.FindByFathers(assign).FindByType(typeof(UnknownReference));

// When the left side is an indexer, use the relevant Unknown reference, instead
CxList assignLeftIndexer = assignLeftSide.FindByType(typeof(IndexerRef));
assignLeftSide -= assignLeftIndexer;
assignLeftSide.Add(All.FindByFathers(assignLeftIndexer).FindByType(typeof(UnknownReference)));

result = assignLeftSide;