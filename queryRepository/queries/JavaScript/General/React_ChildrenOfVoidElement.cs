CxList associativeArrays = Find_AssociativeArrayExpr();
CxList declarators = Find_Declarators();
CxList reactAll = React_Find_All();
CxList reactCreateElement = React_Find_CreateElement();

List<string> voidElementTags = new List<string> {
	"area", "base", "br", "col", "embed", "hr", "img", "input", "keygen",
	"link", "menuitem", "meta", "param", "source", "track", "wbr"
};

CxList htmlElements = reactAll.GetParameters(reactCreateElement, 0);
CxList voidHtmlElements = htmlElements.FindByShortNames(voidElementTags);

CxList createVoidElement = reactCreateElement.FindByParameters(voidHtmlElements);
CxList createVoidElemAttributes = associativeArrays.GetParameters(createVoidElement, 1);

CxList childrenField = declarators.GetByAncs(createVoidElemAttributes).FindByShortName("children");
result = childrenField.GetAncOfType(typeof(Param));