CxList methods = Find_Methods();
CxList interactiveInput = Find_Interactive_Inputs();

CxList ReflectionObjs = All.FindByType("Reflection*");

CxList InvokeTargets = methods.FindByShortName("invoke*").GetTargetOfMembers();

// Tainted Class names 
CxList reflectionClsObj = ReflectionObjs.FindByType("ReflectionClass");
CxList getNewInstanceTargets = methods.FindByShortName("newInstance*").GetTargetOfMembers();
CxList taintedreflectionClsObj = (reflectionClsObj * getNewInstanceTargets).DataInfluencedBy(interactiveInput);
result.Add(taintedreflectionClsObj);

// Tainted Function names
CxList reflectionFuncObj = ReflectionObjs.FindByType("ReflectionFunction");
CxList tainedReflectionFuncObj = (reflectionFuncObj * InvokeTargets).DataInfluencedBy(interactiveInput);
result.Add(tainedReflectionFuncObj);
	
// Tainted Method names
CxList reflectionMethodObj = ReflectionObjs.FindByType("ReflectionMethod");
reflectionMethodObj.Add(reflectionClsObj);
reflectionMethodObj.Add(All.FindByType("CxObject"));

CxList getMethodTargets = methods.FindByShortName("getMethod*").GetTargetOfMembers() * reflectionMethodObj;

CxList affectedInvoke = methods.FindByShortName("invoke*").InfluencedBy(getMethodTargets);
CxList taintedInvoke = affectedInvoke.DataInfluencedBy(interactiveInput);
result.Add(taintedInvoke);