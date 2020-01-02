CxList db = Find_DB();
CxList strings = Find_Strings();

CxList protectedContent = Find_Django_XSRF_Sanitize();

CxList requests = Find_Interactive_Inputs();

List<string> updatesMethodsList = new List<string>{"*update*", "*delete*","*insert*","*save*"};

CxList dbUpdates = db.FindByShortNames(updatesMethodsList);

CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase);
write.Add(strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase));
write.Add(strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase)); 
write.Add(strings.FindByName("*save*", StringComparison.OrdinalIgnoreCase));

//----------- Support for "safe" parameter----------
//Make sure that method "safe" that we will find is called by the right methods
//This method is not safe, it doesn't escape the input
CxList methods = Find_Methods();
List<string> unsafeMethodsList = new List<string>{"safe", "safeseq"};
CxList unsafeMethods = methods.FindByShortNames(unsafeMethodsList, false);
CxList autoEscape = methods.FindByShortName("autoescape", false);
CxList unsafeMethod = unsafeMethods.GetByAncs(autoEscape);

//---------------------------------------------------

result = db.DataInfluencedBy(write).InfluencedByAndNotSanitized(requests, protectedContent);
result.Add(dbUpdates.InfluencedByAndNotSanitized(requests, protectedContent));
result.Add(requests.InfluencingOnAndNotSanitized(unsafeMethod, protectedContent));