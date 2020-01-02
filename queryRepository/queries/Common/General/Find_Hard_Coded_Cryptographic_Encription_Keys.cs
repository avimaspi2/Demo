List<string> names = new List<string>() {"*encr*", "*secret*", "*keyenc*", "*enckey*"};

CxList keys = All.FindByShortName("*key*", false);
result = keys.FindByShortNames(names, false);