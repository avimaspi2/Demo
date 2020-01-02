CxList boundedCopy = Find_Bounded_Methods() + Find_Methods().FindByShortName("bcopy");
CxList _memccpy = Find_Methods().FindByShortName("_memccpy");
CxList sizeParam = All.GetParameters(boundedCopy - _memccpy, 2);
sizeParam.Add(All.GetParameters(_memccpy, 3));
result = sizeParam;