CxList objCreate = Find_Object_Create();
CxList fileOpen = objCreate.FindByShortName("File*");
fileOpen.Add(objCreate.FindByShortName("*.File*"));


//File open in Scala
CxList methods = Find_Methods();
fileOpen.Add(methods.FindByShortName("fromFile"));


result = fileOpen;