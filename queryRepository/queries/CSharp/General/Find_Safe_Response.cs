// All response that has content type which is not text/html.
// Default content type is text/html, so response with no content type at all is not safe are not safe.

CxList strings = Find_Strings();
CxList contentType = All.FindByMemberAccess("response.ContentType");
CxList contentTypeString = strings.GetByAncs(contentType.GetAncOfType(typeof(AssignExpr)));
CxList safeStrings = contentTypeString - contentTypeString.FindByShortName("text/html");
CxList response = Find_Response();
CxList safeContentType = contentType.
	InfluencedByAndNotSanitized(safeStrings, All.FindByType(typeof(BinaryExpr))).
	GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = response.DataInfluencedBy(safeContentType).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);