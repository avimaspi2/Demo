// $ASP

CxList exec = All.NewCxList();
exec.Add(Find_Member_With_Target("WScript.Shell", "Run"));	
exec.Add(Find_Member_With_Target("WScript.Shell", "Exec"));	
exec.Add(Find_Member_With_Target("WScript.Shell", "AppActivate"));
exec.Add(Find_Member_With_Target("shell.application", "ShellExecute"));
    
result = exec;