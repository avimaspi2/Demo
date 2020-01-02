CxList razorHelpers = Find_ASP_MVC_HtmlHelper_Methods();

result = 
	razorHelpers
	.FindByShortNames(
	new List<string>(){"Display", "DisplayFor", "DisplayForModel",
		"DisplayText", "DisplayTextFor", "Label", "LabelFor", "Raw",
		"ActionLink"
		});
result.Add(Find_ASP_MVC_HtmlHelper_InputsOutputs());

// Custom HTML Helper to outputs
result.Add(Find_ASP_MVC_HtmlHelper_Custom());