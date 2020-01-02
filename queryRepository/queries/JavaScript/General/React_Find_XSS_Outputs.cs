List <string> xssOutputs = new List<string> {"dangerouslySetInnerHTML", "href"};
result = React_Find_PropertyKeys().FindByShortNames(xssOutputs);