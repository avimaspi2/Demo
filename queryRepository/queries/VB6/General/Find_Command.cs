CxList methods = Find_Methods();

result = methods.FindByShortName("CreateProcess", false); 
result.Add(methods.FindByShortName("CreateProcessA", false)); 
result.Add(methods.FindByShortName("Shell", false)); 
result.Add(Find_Member_With_Target("wscript.shell", "run"));