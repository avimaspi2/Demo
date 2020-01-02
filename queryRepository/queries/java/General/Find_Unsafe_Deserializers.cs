CxList inheritsFrom = All.InheritsFrom("ObjectInputStream");


CxList methods = Find_Methods();
CxList methodsDecl = Find_MethodDeclaration().FindByShortName("resolveClass");
CxList readObject = methods.FindByMemberAccess("ObjectInputStream.readObject");
readObject.Add(methods.FindByMemberAccess("Unmarshaller.readObject"));
readObject.Add(methods.FindByMemberAccess("XMLDecoder.readObject"));

//XStream library deserialization methods
readObject.Add(Find_XStream_Deserialization_Methods());

foreach (CxList l in inheritsFrom)
{
	CxList res = methodsDecl.GetByAncs(l);
	if (res.Count > 0)
	{
		CxList temp = methods.FindByMemberAccess(l.GetName() + ".read*");
		readObject -= temp;
	}
}

result = readObject;