if(Lightning_Find_Aura_Cmp_And_App().Count > 0)
{
	CxList inputs = Lightning_Find_CS_Inputs();
	inputs.Add(Lightning_Find_Aura_Enabled_Inputs());
	CxList outputs = Lightning_Find_Outputs_XSS();
	CxList sanitize = Sanitize();
	sanitize.Add(Find_XSS_Sanitize());
	sanitize.Add(Lightning_XSS_Sanitizer());
	result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize);
}