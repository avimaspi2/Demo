CxList outputs = All.NewCxList();
	
if(param.Length == 1 && (param[0] as CxList).Count > 0){

	outputs = param[0] as CxList;
	
	CxList inputs = Find_Interactive_Inputs();
	//CxList outputs = Find_XSS_Outputs();

	//Decode methods from java.util.Base64.Decoder()
	CxList util_decode = All.FindByName("*Base64.Decoder").GetMembersOfTarget().FindByShortName("decode*");

	util_decode.Add(All.FindByName("*Base64.getDecoder").GetMembersOfTarget().FindByShortName("decode*"));
	util_decode.Add(All.FindByName("*Base64.getMimeDecoder").GetMembersOfTarget().FindByShortName("decode*"));
	util_decode.Add(All.FindByName("*Base64.getUrlDecoder").GetMembersOfTarget().FindByShortName("decode*"));
	//Encode methods from sun.misc.BASE64Encoder
	CxList misc_decode = All.FindByMemberAccess("BASE64Decoder.decode*");

	CxList scalaBase64Decode = All.FindByMemberAccess("Base64.decodeString");
	scalaBase64Decode.Add(All.FindByMemberAccess("Base64.decode*"));
	scalaBase64Decode.Add(All.FindByMemberAccess("apache64.decodeBase64*"));

	CxList decode = util_decode + misc_decode + scalaBase64Decode;


	//Encode methods from java.util.Base64.Encoder()
	CxList util_encode = All.FindByName("*Base64.Encoder").GetMembersOfTarget().FindByShortName("encode*");
	util_encode.Add(All.FindByName("*Base64.getEncoder").GetMembersOfTarget().FindByShortName("encode*"));
	util_encode.Add(All.FindByName("*Base64.getMimeEncoder").GetMembersOfTarget().FindByShortName("encode*"));
	util_encode.Add(All.FindByName("*Base64.getUrlEncoder").GetMembersOfTarget().FindByShortName("encode*"));
	//Encode methods from sun.misc.BASE64Encoder
	CxList misc_encode = All.FindByMemberAccess("BASE64Encoder.encode*");

	CxList scalaBase64Encode = All.FindByMemberAccess("Base64.encodeString");
	scalaBase64Encode.Add(All.FindByMemberAccess("Base64.encode*"));
	scalaBase64Encode.Add(All.FindByMemberAccess("apache64.encodeBase64*"));

	CxList encode = util_encode + misc_encode + scalaBase64Encode;

	decode = decode.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	encode = encode.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	encode = decode.DataInfluencedBy(encode);
	encode = encode.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

	result = encode.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
}