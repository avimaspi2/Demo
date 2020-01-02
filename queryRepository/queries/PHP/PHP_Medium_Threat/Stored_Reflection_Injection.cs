CxList methods = Find_Methods();
CxList db = Find_DB_Out() + Find_Read();

CxList ReflectionObjs = All.FindByType("Reflection*");
CxList invokers = methods.FindByShortName("invoke*");
CxList InvokeTargets = invokers.GetTargetOfMembers();

// Tainted Class names 
CxList reflectionClsObj = ReflectionObjs.FindByType("ReflectionClass");
CxList getNewInstanceTargets = methods.FindByShortName("newInstance*").GetTargetOfMembers();
CxList taintedreflectionClsObj = (reflectionClsObj * getNewInstanceTargets).DataInfluencedBy(db);
result.Add(taintedreflectionClsObj);

// Tainted Function names
CxList reflectionFuncObj = ReflectionObjs.FindByType("ReflectionFunction");
CxList tainedReflectionFuncObj = (reflectionFuncObj * InvokeTargets).DataInfluencedBy(db);
result.Add(tainedReflectionFuncObj);
	
// Tainted Method names
CxList reflectionMethodObj = ReflectionObjs.FindByType("ReflectionMethod");
reflectionMethodObj.Add(reflectionClsObj);
reflectionMethodObj.Add(All.FindByType("CxObject"));

CxList getMethodTargets = methods.FindByShortName("getMethod*").GetTargetOfMembers() * reflectionMethodObj;

CxList affectedInvoke = invokers.InfluencedBy(getMethodTargets);
CxList taintedInvoke = affectedInvoke.DataInfluencedBy(db);
result.Add(taintedInvoke);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);