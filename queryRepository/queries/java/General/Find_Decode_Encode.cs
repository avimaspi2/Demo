if(param.Length == 1 && (param[0] as CxList).Count > 0){

	CxList methods = Find_Methods();
	CxList unknownRef = Find_UnknownReference();
	CxList outputs = param[0] as CxList;	
	CxList inputs = Find_Interactive_Inputs();

	//Decode methods from java.util.Base64.Decoder()
	CxList utilDecode = unknownRef.FindByType("Base64.Decoder");
	utilDecode.Add(methods.FindByMemberAccess("Base64.getDecoder"));
	utilDecode.Add(methods.FindByMemberAccess("Base64.getUrlDecoder"));
	utilDecode.Add(methods.FindByMemberAccess("Base64.getMimeDecoder"));
	CxList utilDecodeMethods = utilDecode.GetMembersOfTarget().FindByShortName("decode*");
	//Decode methods from sun.misc.BASE64Encoder
	CxList miscDecode = methods.FindByMemberAccess("BASE64Decoder.decode*");
	//Decode methods from Apache Base64
	CxList apacheDecode = methods.FindByMemberAccess("Base64.decodeBase64");

	CxList decode = utilDecodeMethods;
	decode.Add(miscDecode);
	decode.Add(apacheDecode);

	//Encode methods from java.util.Base64.Encoder()
	CxList utilEncode = unknownRef.FindByType("Base64.Encoder");
	utilEncode.Add(methods.FindByMemberAccess("Base64.getEncoder"));
	utilEncode.Add(methods.FindByMemberAccess("Base64.getUrlEncoder"));
	utilEncode.Add(methods.FindByMemberAccess("Base64.getMimeEncoder"));
	CxList utilEncodeMethods = utilEncode.GetMembersOfTarget().FindByShortName("encode*");
	//Encode methods from sun.misc.BASE64Encoder
	CxList miscEncode = methods.FindByMemberAccess("BASE64Encoder.encode");
	//Encode methods from Apache Base64
	CxList apacheEncode = methods.FindByMemberAccess("Base64.encodeBase64");

	CxList encode = utilEncodeMethods;
	encode.Add(miscEncode);
	encode.Add(apacheEncode);
	
	decode = decode.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	encode = encode.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	encode = decode.DataInfluencedBy(encode);
	encode = encode.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

	result = encode.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
}