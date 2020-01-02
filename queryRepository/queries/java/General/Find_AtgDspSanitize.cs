// to add customization of bean names user should implement the following procedure
// 1. Override this query to All projects under group
// 2. add the following lines to the Override query, after the default content:
//
// List < string > nonVulnBean = new List<string>(new string [] { "<sanitizer bean name 1>", "<sanitizer bean name 2>", .. "<sanitizer bean name N>"});
// CxList AtgDspSanitizer = Add_Get_Dsp_Sanitize(nonVulnBean);
// result.Add(AtgDspSanitizer);
//
// where <sanitizer bean name 1>, <sanitizer bean name 2>, .. <sanitizer bean name N> are names of non vulnerable beans



CxList AllDsp = Get_AllDSP();
if (AllDsp.Count > 0)
{
	
	CxList methodInvoke = Find_Methods() * AllDsp;
	CxList DSP = methodInvoke.FindByShortName("*__DSP__*");	

	result = DSP.FindByShortName("Sanitize__DSP__");

	// find all tag converter that sanitizers
	CxList converterByName = DSP.FindByShortNames(new List<string> {
			"__DSP__currency",
			"__DSP__currencyConversion",
			"__DSP__euro",
			"__DSP__creditcard",
			"__DSP__number",
			"__DSP__date"});

	result.Add(converterByName);


	// find valueishtml(false) and valueishtml("false") as sanitizer
	CxList valueishtml = DSP.FindByShortName("__DSP__valueishtml");
	CxList valueishtmlArg = AllDsp.GetParameters(valueishtml);
	CxList sanitizesValueishtml = valueishtmlArg.FindByShortNames(new List<string> {"false", "\"false\""});

	sanitizesValueishtml = methodInvoke.FindByParameters(sanitizesValueishtml);

	result.Add(sanitizesValueishtml);
}