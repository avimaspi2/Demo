//vlaue
//CCCrypt, 4
//CCCryptorCreate, 4

//length
CxList methods = Find_Methods();

List<string> methodsNames = new List<string>() {"CCCrypt", "CCCryptorCreate"};
CxList crypt = methods.FindByShortNames(methodsNames);

result = All.GetParameters(crypt, 4);
//CCCrypt, 5
//CCCryptorCreate, 5