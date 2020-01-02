// find all the references from a deserializer initialization
CxList desserializers = Find_Deserializers_Constructors();
CxList desserializersRefs = Find_UnknownReferences()
	.FindAllReferences(desserializers.GetAncOfType(typeof(Declarator)));

// Find the struct that invoke the method Decode (which corresponds to the deserialization)
CxList readStruct = desserializersRefs.GetMembersOfTarget().FindByShortName("Decode");

result = readStruct;

List<string> methods = new List<string> {"Unmarshal","UnmarshalWithParams"};

// Unmarshal methods from the json, xml and asnl interface are other different ways to
// deserialize data
CxList unmarshal = All.NewCxList();
unmarshal.Add(All.FindByMemberAccess("\"encoding/json\".Unmarshal"));
unmarshal.Add(All.FindByMemberAccess("\"encoding/xml\".Unmarshal"));
unmarshal.Add(All.FindByMemberAccess("\"encoding/asn1\".*").FindByShortNames(methods));

result.Add(unmarshal);