CxList methods = Find_Methods();
result.Add(methods.FindByMemberAccess("XPathNavigator.Compile"));
result.Add(methods.FindByMemberAccess("XPathNavigator.Select*"));
result.Add(methods.FindByMemberAccess("XPathNavigator.Evaluate"));
result.Add(methods.FindByMemberAccess("XmlDocument.SelectNodes"));
result.Add(methods.FindByMemberAccess("XmlDocument.SelectSingleNode"));