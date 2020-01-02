CxList refl =
	All.FindByName("Class.forName") + 
	All.FindByName("*.Class.forName") +
	All.FindByMemberAccess("Class.getMethod") + 
	All.FindByName("System.loadLibrary") + 
	// Groovy Parse Class
	All.FindByName("*.parseClass");

CxList script = 
	All.FindByMemberAccess("ScriptEngine.eval") + 
	All.FindByMemberAccess("getEngineByMimeType.eval") +
	All.FindByMemberAccess("getEngineByName.eval") +
	All.FindByMemberAccess("getEngineByExtension.eval") +
	All.FindByMemberAccess("Eval.*") +
	All.FindByMemberAccess("GroovyShell.*");

result = refl + script;