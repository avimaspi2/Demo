CxList encode = Find_Methods().FindByShortNames(new List<string>{
		"*HTMLencode*", "GetSafeHtmlFragment"}, false);

//All Razor helpers except @Html.Raw are encoded an therefore sanitizers
CxList razorInputs = Find_ASP_MVC_HtmlHelper_Inputs();
CxList razorOutputs = Find_ASP_MVC_HtmlHelper_Outputs();
encode.Add(razorInputs);
encode.Add(razorOutputs-razorOutputs.FindByShortName("Raw"));

result = encode;