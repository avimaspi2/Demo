CxList methods = Find_Methods();
CxList methodDecl = All.FindByType(typeof(MethodDecl));

CxList deserialize = methods.FindByMemberAccess("BinaryFormatter.Deserialize");
deserialize.Add(methods.FindByMemberAccess("BinaryFormatter.DeserializeMethodResponse"));
deserialize.Add(methods.FindByMemberAccess("BinaryFormatter.UnsafeDeserialize"));
deserialize.Add(methods.FindByMemberAccess("BinaryFormatter.UnsafeDeserializeMethodResponse"));
deserialize.Add(methods.FindByMemberAccess("SoapFormatter.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("XmlSerializer.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("IFormatter.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("XmlObjectSerializer.ReadObject"));
deserialize.Add(methods.FindByMemberAccess("DataContractSerializer.ReadObject"));
deserialize.Add(methods.FindByMemberAccess("DataContractJsonSerializer.ReadObject"));
deserialize.Add(methods.FindByMemberAccess("Marshal.Read*"));	
deserialize.Add(methods.FindByMemberAccess("Marshal.PtrToStructure"));
deserialize.Add(methods.FindByMemberAccess("JSON.ToObject"));
deserialize.Add(methods.FindByMemberAccess("LosFormatter.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("NetDataContractSerializer.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("ObjectStateFormatter.Deserialize"));

CxList newtonSoftJsonDeserializer = methods.FindByMemberAccess("JsonConvert.DeserializeObject");
//Deserialization calls with a single parameter use the default settings which are safe.
deserialize.Add(newtonSoftJsonDeserializer.FindByParameters(All.GetParameters(newtonSoftJsonDeserializer, 1)));


CxList iformatter = All.InheritsFrom("IFormatter");
CxList iformatterMethods = methodDecl.GetByAncs(iformatter);
CxList iformatterDeserialize = iformatterMethods.FindByShortName("Deserialize");
deserialize.Add(methods.FindAllReferences(iformatterDeserialize));

CxList iserializable = All.InheritsFrom("ISerializable");
CxList iserializableMethods = methodDecl.GetByAncs(iserializable);
CxList iiserializableGetObject = iserializableMethods.FindByShortName("GetObjectData");
deserialize.Add(methods.FindAllReferences(iiserializableGetObject));


CxList JavaScriptSerializerCreations = All.FindByShortName("JavaScriptSerializer").FindByType(typeof(ObjectCreateExpr));
CxList JavaScriptSerializer = All.NewCxList();
foreach (CxList JavaScriptSerializerCreation in JavaScriptSerializerCreations)
{
	CxList parameters = All.GetParameters(JavaScriptSerializerCreation);
	if (parameters.Count > 0)
	{
		JavaScriptSerializer.Add(JavaScriptSerializerCreation);
	}
}

CxList AllJavaScriptSerializerDeserialize = methods.FindByMemberAccess("JavaScriptSerializer.Deserialize");
deserialize.Add( AllJavaScriptSerializerDeserialize.InfluencedBy(JavaScriptSerializer.GetFathers()).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));




result = deserialize;