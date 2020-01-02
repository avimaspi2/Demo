CxList inputStream = All.FindByType("ObjectInputStream");
CxList inheritsFrom = All.InheritsFrom("ObjectInputStream");

inheritsFrom -= All.FindByType(typeof(MethodDecl)).
	FindByShortName("resolveClass").
	GetAncOfType(typeof(ClassDecl));

CxList readObject = inputStream.GetMembersOfTarget().FindByShortName("read*");
foreach(CxList x in inheritsFrom){
	readObject += All.FindByType(x.GetName()).GetMembersOfTarget().FindByShortName("read*");
}

//XStream library deserialization methods
readObject.Add(Find_XStream_Deserialization_Methods());


result = readObject;