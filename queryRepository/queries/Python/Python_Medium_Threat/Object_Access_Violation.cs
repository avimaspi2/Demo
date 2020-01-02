//Finds unsanatized user inputs to attribute functions
CxList methods = Find_Methods();
CxList inputs = Find_Inputs();
CxList Sanitize = Find_Sanitize();
//The following are the attr methods that generate this vulnerability
List<string> attrMethodNames = new List<string> {"getattr", "setattr", "hasattr", "delattr"};
CxList attrMethods = methods.FindByShortNames(attrMethodNames);

result = attrMethods.InfluencedByAndNotSanitized(inputs, Sanitize);