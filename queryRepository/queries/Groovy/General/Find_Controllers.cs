result = 
	All.FindByCustomAttribute("Controller").GetFathers().FindByType(typeof(ClassDecl)) +
	All.InheritsFrom("SimpleFormController") + 
	All.InheritsFrom("Controller") + 
	All.InheritsFrom("AbstractWizardFormController") + 
	All.InheritsFrom("SimpleControllerHandlerAdapter") + 
	All.InheritsFrom("AbstractController") + 
	All.InheritsFrom("AbstractCommandController") + 
	All.InheritsFrom("MultiActionController") + 
	All.InheritsFrom("AbstractFormController");