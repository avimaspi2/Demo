if(ReactNative_Find_Presence().Count > 0){
	CxList inputs = ReactNative_Find_Inputs();
	CxList outputs = ReactNative_Find_Outputs();
	outputs = outputs.FindByMemberAccess("Clipboard.setString");

	result = inputs.InfluencingOn(outputs);
}