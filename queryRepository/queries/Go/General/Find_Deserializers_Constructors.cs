//All Desserilizers found in
// https://golang.org/pkg/encoding/
CxList desserializers = All.NewCxList();
desserializers.Add(All.FindByMemberAccess("\"io\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/gob\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/ascii85\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/base32\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/base64\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/hex\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/json\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/pem\".NewDecoder"));
desserializers.Add(All.FindByMemberAccess("\"encoding/xml\".NewDecoder"));

result = desserializers;