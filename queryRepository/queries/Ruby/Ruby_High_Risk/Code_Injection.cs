CxList methods = Find_Methods();
CxList dynamic_method_invoke = methods.FindByShortName("eval");
CxList inputs = Find_Interactive_Inputs();

result = inputs.DataInfluencingOn(dynamic_method_invoke);