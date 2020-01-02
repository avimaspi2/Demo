// Find all decodeObjectForKey usages in initWithCoder methods implementation in classes inherits from NSSecureCoding 
CxList decodeObjectForKeyInInitWithCoderInSecuredClass = All.NewCxList();
try
{
	CxList methods = Find_Methods();	
	CxList securedClasses = Find_Secured_Classes();	
	List<string> allMethodConst = new List<string> {"*initWithCoder:*", "init:"};	
	CxList initInCoders = All.GetByAncs(securedClasses).FindByType(typeof(MethodDecl)).FindByShortNames(allMethodConst);	
	decodeObjectForKeyInInitWithCoderInSecuredClass = methods.GetByAncs(initInCoders).FindByShortName("decodeObjectForKey:*");
	decodeObjectForKeyInInitWithCoderInSecuredClass.Add(methods.GetByAncs(initInCoders).FindByShortName("decodeObject:*").FindByParameterName("forKey", 0));	

}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = decodeObjectForKeyInInitWithCoderInSecuredClass;