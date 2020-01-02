CxList classes = Find_Class_Decl();

result =
	classes.InheritsFrom("Action") + 
	classes.InheritsFrom("DispatchActionSupport") + 
	classes.InheritsFrom("ModelDriven") + 
	classes.InheritsFrom("ActionSupport") + 
	classes.InheritsFrom("LookupDispatchActionSupport") + 
	classes.InheritsFrom("MappingDispatchActionSupport") +
	classes.InheritsFrom("EventDispatchAction") +
	classes.InheritsFrom("LookupDispatchAction") +
	classes.InheritsFrom("MappingDispatchAction") +
	classes.InheritsFrom("DefinitionDispatcherAction") +
	classes.InheritsFrom("DispatchAction") +
	classes.InheritsFrom("DownloadAction") +
	classes.InheritsFrom("ForwardAction") +
	classes.InheritsFrom("IncludeAction") +
	classes.InheritsFrom("LocaleAction") +
	classes.InheritsFrom("ReloadDefinitionsAction") +
	classes.InheritsFrom("SwitchAction") +
	classes.InheritsFrom("TilesAction") +
	classes.InheritsFrom("ViewDefinitionsAction") +
	classes.InheritsFrom("DelegatingActionProxy") +
	classes.InheritsFrom("BaseAction") +
	classes.InheritsFrom("MockAction");