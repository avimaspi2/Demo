if(cxScan.IsFrameworkActive("Handlebars"))
{
	string TEMPLATE_SUFIX = "*_HBTemplate";
	string HANDLEBARS_COMPILE = "*Handlebars.compile*";

	CxList methods = Find_Methods();
	
	/************ Finding Handlebars templates ************/
	CxList allTemplateDefinitions = Find_MethodDecls().FindByShortName(TEMPLATE_SUFIX);

	/************ Finding Handlebars template calls (and their definition) ************/
	CxList invokedTemplates = All.NewCxList();

	// Find Handlebars.compile
	CxList compileMethods = methods.FindByName(HANDLEBARS_COMPILE);

	// For each Compile method
	foreach(CxList compileMethod in compileMethods)
	{
		// Find the variables that gather the Handlebars compiler
		CxList assignedCompilers = compileMethod.GetAssignee();
	
		// For each Compile variable
		foreach(CxList compilerVariable in assignedCompilers)
		{
			// Find the usages of Handlebars compiler
			CxList compilerReferences = methods.FindAllReferences(compilerVariable);
	
			// If found an Handlebars compile executing mark it and relate it to a compile "constructor"!
			if (compilerReferences.Count > 0) {
				invokedTemplates.Add(All.FindDefinition(compilerReferences));
				break;		
			}
		}
	}

	// Take all the template definitions and remove the called ones
	result = allTemplateDefinitions - invokedTemplates;
}