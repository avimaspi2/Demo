// we're looking for things like credentials = "username:password"

// find all the credentails
CxList credentailsStrings = Find_Strings().FindByShortName("*:*");

CxList credentails = All.NewCxList();
credentails.Add(credentailsStrings);
credentails -= credentailsStrings.FindByShortName(":*");
credentails -= credentailsStrings.FindByShortName("*:");

// find all the string varaibles
CxList vars = credentails.GetAssignee(All);
CxList news = credentails.GetAncOfType(typeof(ArrayCreateExpr)); // when the strings is part of an array
vars.Add(news.GetAssignee(All));

// make sure the vars are of type credetials or password
CxList passwords = (Find_All_Passwords() * vars);
vars = vars.FindByShortName("*cred*", false);
vars.Add(passwords);

result = vars;