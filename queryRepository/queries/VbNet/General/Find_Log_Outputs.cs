result =   All.FindByName("*Debug.Write*", false);
result.Add(All.FindByName("*Trace.Write*", false)); 
result.Add(All.FindByName("*Debug.WriteLine", false));
result.Add(All.FindByName("*Trace.WriteLine", false)); 
result.Add(All.FindByName("*Log.WriteLine", false));
result.Add(All.FindByName("*Log.Write*", false));
result.Add(All.FindByMemberAccess("LogWriter.Write", false)); // EndLib
result.Add(All.FindByName("*Logger.Write", false));	// EntLib;