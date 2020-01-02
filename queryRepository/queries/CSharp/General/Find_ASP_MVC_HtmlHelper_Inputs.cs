CxList razorHelpers = Find_ASP_MVC_HtmlHelper_Methods();

result = 
	razorHelpers
	.FindByShortNames(
	new List<string>(){"CheckBox", "CheckBoxFor", "RadioButton",
		"RadioButtonFor", "DropDownList", "DropDownListFor",
		"Hidden", "HiddenFor", "Password", "PasswordFor", 
		"Editor", "EditorFor", "EditorForModel","EnumDropDownListFor", 
		"ListBox", "ListBoxFor"
		});
result.Add(Find_ASP_MVC_HtmlHelper_InputsOutputs());

// Custom HTML Helper to inputs
result.Add(Find_ASP_MVC_HtmlHelper_Custom());