//pelase see http://www.ruby-doc.org/core-2.0/doc/security_rdoc.html
CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();
CxList dangerousMethods = methods.FindByShortName("const_get") +
	methods.FindByShortName("instance_variable_get") +
	methods.FindByShortName("_set");
	
result= dangerousMethods.DataInfluencedBy(inputs);