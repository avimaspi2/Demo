// Any sensitive data: variables named password, credit-card, etc.

CxList declarators = Find_Declarators();

// Passwords
List<string> pswdIncludeList = new List<string>{"*password*", 
		"*psw", "psw*", "pwd*", "*pwd", "*authKey*",
		"pass*", "cipher*", "*cipher"};

CxList pswdResults = declarators.FindByShortNames(pswdIncludeList, false);

// CreditCards
List<string> creditCardIncludeList = new List<string>{"*creditCard*"};
CxList creditCardResults = declarators.FindByShortNames(creditCardIncludeList, false);

result.Add(pswdResults);
result.Add(creditCardResults);