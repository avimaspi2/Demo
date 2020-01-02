CxList refl = All.FindByName("Class.forName");
refl.Add(All.FindByName("*.Class.forName"));
refl.Add(All.FindByMemberAccess("Class.getMethod"));
refl.Add(All.FindByName("System.loadLibrary"));
refl.Add(All.FindByName("System.load"));

CxList script = All.FindByMemberAccess("ScriptEngine.eval");
script.Add(All.FindByMemberAccess("getEngineByMimeType.eval"));
script.Add(All.FindByMemberAccess("getEngineByName.eval"));
script.Add(All.FindByMemberAccess("getEngineByExtension.eval"));

CxList load = All.FindByMemberAccess("ObjectInputStream.readObject");

//Third-party libraries (commonly used)
//Twitter: ex. (new Eval()).apply(code)
CxList thirdParty = All.FindByMemberAccess("Eval.apply");

result = refl;
result.Add(script);
result.Add(load);
result.Add(thirdParty);