// Encrypt::instance($this->_encrypted)->encode($data);
CxList encryptObject = All.FindByMemberAccess("Encrypt.instance", false);
CxList encryptEncode = encryptObject.GetMembersOfTarget().FindByShortName("encode", false);
result.Add(encryptEncode);

//$encrypt = Encrypt::instance('tripledes');
//$encrypted_data = encrypt->encode('Data to Encode');
CxList encryptObjectAssignee = encryptObject.GetAssignee();
CxList encryptObjectAssigneeAllReferences = All.FindAllReferences(encryptObjectAssignee);
CxList encode = encryptObjectAssigneeAllReferences.GetMembersOfTarget().FindByShortName("encode", false);
result.Add(encode);