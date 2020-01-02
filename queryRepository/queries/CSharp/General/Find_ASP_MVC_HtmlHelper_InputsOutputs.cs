CxList razorHelpers = Find_ASP_MVC_HtmlHelper_Methods();

result = 
	razorHelpers
	.FindByShortNames(new List<string>(){"TextBox","TextBoxFor",
		"TextArea", "TextAreaFor", "Password", "PasswordFor"});