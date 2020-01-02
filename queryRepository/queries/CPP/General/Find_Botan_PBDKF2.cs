//Get the PBDKF2 retrievers
CxList get_pbkdf = Find_Methods().FindByShortNames(new List<string> {"get_pbkdf","get_s2k"});

//Get only pbdkf2 functions that receive a string containing "PBKDF2"
CxList parameters = All.GetParameters(get_pbkdf, 0);
CxList strings = parameters.FindByAbstractValue(x => x is StringAbstractValue);
CxList pbdkf2 = strings.FindByAbstractValue(x => (x as StringAbstractValue).Content.StartsWith("PBKDF2"));
result = get_pbkdf.FindByParameters(pbdkf2);