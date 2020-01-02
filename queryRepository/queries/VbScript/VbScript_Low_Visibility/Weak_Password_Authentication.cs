CxList open = Find_XmlHttp_Open();
result = All.GetParameters(open, 4).FindByType(typeof(StringLiteral));
result = open.DataInfluencedBy(result);