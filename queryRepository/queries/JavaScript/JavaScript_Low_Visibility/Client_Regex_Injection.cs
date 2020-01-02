CxList inputs = Find_Inputs();
CxList sanitize = Sanitize();
/*
//1. regex declaration of type ==> var patt=/pattern/modifiers;
//currently the query doesn't deal with this type of regex declaration
CxList methods = Find_Methods();
CxList methodRegs = methods.FindByShortName("RegExp");	
*/

//2. regex declaration of type ==> var patt=new RegExp(pattern,modifiers);
CxList newReg = Find_ObjectCreations();
newReg = newReg.FindByShortName("RegExp");
CxList theReg = All.GetParameters(newReg, 0);// +All.GetParameters(methodRegs);
CxList regFromInput = theReg.InfluencedByAndNotSanitized(inputs, sanitize);
result.Add(regFromInput);

//3. RegExp 
CxList methods = Find_Methods();
CxList execOrTest = methods.FindByShortNames(new List<string>{"exec","test"});
CxList regex = execOrTest.GetTargetOfMembers();
result.Add(inputs * regex);
result.Add(inputs.DataInfluencingOn(regex));