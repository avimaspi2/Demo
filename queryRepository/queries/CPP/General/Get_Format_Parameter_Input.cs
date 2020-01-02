CxList func1 = 	Find_Methods().FindByShortName("scanf");
CxList firstParam = All.GetParameters(func1, 0);

CxList func2 = 	Find_Methods().FindByShortName("sscanf");
CxList secondParam = All.GetParameters(func2, 1);

result = firstParam + secondParam;