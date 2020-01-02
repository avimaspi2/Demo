if(cxScan.IsFrameworkActive("AngularJS"))
{
	CxList outputs = Find_Outputs_XSS();
	CxList inputs = Find_Inputs_NoWindowLocation();
	CxList unsafeData = AngularJS_Find_Unsafe_Data();

	CxList sanitize = Sanitize();
	sanitize.Add(Find_XSS_Sanitize());
	sanitize -= AngularJS_Find_Outputs_XSS();

	CxList trustedOutputs = unsafeData.InfluencingOnAndNotSanitized(outputs, sanitize);
	result = trustedOutputs.DataInfluencedBy(inputs);
}