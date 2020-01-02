CxList methods = Find_Methods();
string[] encryptions = {"CCCrypt", "CCCryptorCreate", "CCCryptorCreateFromData"};
result.Add(methods.FindByShortNames(new List<string>(encryptions)));