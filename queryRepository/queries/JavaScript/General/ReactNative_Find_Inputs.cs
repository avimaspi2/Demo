CxList methods = Find_Methods();
CxList objs = Find_ObjectCreations();
CxList fields = Find_FieldDecls();
CxList paramDecls = Find_ParamDecl();

result.Add(methods.FindByMemberAccess("Clipboard.getString"));
result.Add(methods.FindByMemberAccess("Linking.addEventListener"));
result.Add(methods.FindByMemberAccess("Linking.getInitialURL"));
result.Add(methods.FindByMemberAccess("AsyncStorage.getItem"));
result.Add(methods.FindByMemberAccess("AsyncStorage.multiGet"));
CxList textInputRefs = objs.FindByShortName("TextInput");
result.Add(textInputRefs);
/*
Find all the parameters of lambda expressions associated to 
- onChangeText
- onSubmitEditting
events
*/
CxList textInputsLambdas = Find_LambdaExpr().GetByAncs(textInputRefs);
CxList textInputsEventProps = fields.GetByAncs(textInputRefs)
										.FindByShortNames(new List<string> {"onChangeText", "onSubmitEditing"});
CxList lambdasOnTextInputEvents = textInputsEventProps.GetAssigner(textInputsLambdas);
CxList inputParameters = paramDecls.GetParameters(lambdasOnTextInputEvents);

result.Add(inputParameters);