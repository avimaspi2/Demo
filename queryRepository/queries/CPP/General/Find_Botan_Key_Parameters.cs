// Key is the 1st parameter in set_key method from object cipher which can be a StreamCipher or a BlockCipher
string[] botan_ciphers = {"Botan.StreamCipher", "Botan.BlockCipher", "StreamCipher", "BlockCipher"};
CxList cipherTypes = Find_TypeRef().FindByTypes(botan_ciphers);
CxList ciphers = cipherTypes.GetAncOfType(typeof(VariableDeclStmt));
ciphers.Add(cipherTypes.GetAncOfType(typeof(FieldDecl)));
CxList variableDeclCiphers =  Find_Declarators().GetByAncs(ciphers);
CxList cipher_refs = All.FindAllReferences(variableDeclCiphers);

CxList set_key_methods = cipher_refs.GetMembersOfTarget().FindByShortName("set_key");

result = All.GetParameters(set_key_methods, 0);
result -= Find_Parameters();