CxList Log = All.FindByName("*debug.write") +
	All.FindByName("*trace.write") + 
	All.FindByName("*log.write", false) + 
	All.FindByName("*debug.writeline") +
	All.FindByName("*trace.writeline") + 
	All.FindByName("*log.writeline", false) +

	All.FindByName("*Response.AppendToLog", false);

result = Log;