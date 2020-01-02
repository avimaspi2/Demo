CxList reactAll = React_Find_All();
CxList strings = Find_String_Literal();
CxList associativeArrays = Find_AssociativeArrayExpr();
CxList declarators = Find_Declarators();
CxList reactAllChildrenProperties = reactAll.FindByShortName("children");
CxList symbolsWithParameters = Find_Methods();
symbolsWithParameters.Add(Find_ObjectCreations());

CxList reactCreateElement = React_Find_CreateElement();

CxList createElementStringFstParam = strings.GetParameters(reactCreateElement, 0);
CxList htmlReactElements = reactCreateElement.FindByParameters(createElementStringFstParam);
CxList reactReactElements = reactCreateElement - htmlReactElements;

CxList htmlReactElementsRelevantParam = associativeArrays.GetParameters(htmlReactElements, 1);
CxList createElementRelevantParameters = symbolsWithParameters.GetByAncs(reactReactElements);
CxList reactReactElementsRelevantParam = associativeArrays.GetParameters(createElementRelevantParameters,0);

CxList relevantParameters = All.NewCxList();
relevantParameters.Add(htmlReactElementsRelevantParam);
relevantParameters.Add(reactReactElementsRelevantParam);

CxList propertyKeys = declarators.GetByAncs(relevantParameters);

result = propertyKeys - reactAllChildrenProperties;