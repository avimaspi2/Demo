CxList methods = Find_Methods();
CxList dynamic_method_invoke = methods.FindByShortName("eval");
CxList inputs = Find_DB() + Find_Read();

result = inputs.DataInfluencingOn(dynamic_method_invoke);