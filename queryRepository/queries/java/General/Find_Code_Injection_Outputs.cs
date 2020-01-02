CxList refl = All.FindByName("Class.forName");
refl.Add(All.FindByName("*.Class.forName"));
refl.Add(All.FindByMemberAccess("Class.getMethod"));
refl.Add(All.FindByName("System.loadLibrary"));
refl.Add(All.FindByName("System.load"));
refl.Add(All.FindByMemberAccess("ClassLoader.loadClass"));
refl.Add(All.FindByMemberAccess("DexClassLoader.loadClass"));
refl.Add(All.FindByMemberAccess("BaseClassLoader.loadClass"));

CxList script = All.FindByMemberAccess("ScriptEngine.eval");
script.Add(All.FindByMemberAccess("getEngineByMimeType.eval"));
script.Add(All.FindByMemberAccess("getEngineByName.eval"));
script.Add(All.FindByMemberAccess("getEngineByExtension.eval"));

result.Add(refl);
result.Add(script);