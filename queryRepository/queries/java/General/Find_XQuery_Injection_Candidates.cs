CxList methods = Find_Methods();
// Jaxp  
CxList candidates = methods.FindByMemberAccess("XQExpression.execute*");
candidates.Add(methods.FindByMemberAccess("XQPreparedExpression.execute*"));
// Saxon
candidates.Add(methods.FindByMemberAccess("XQueryEvaluator.run*"));
candidates.Add(methods.FindByMemberAccess("XQueryExpression.evaluate*"));

result = candidates;