CxList Log = 
	All.FindByName("*debug.write*", false) +
	All.FindByName("*debug.print*", false) +
	All.FindByName("*debug_print*", false) +
	All.FindByName("*trace.write*", false) + 
	All.FindByName("*trace.print*", false) + 
	All.FindByName("*log.write*", false) + 
	All.FindByName("*logger.write*", false) + 
	All.FindByName("*logwrite*", false) + 
	All.FindByName("*log_write*", false) + 
	All.FindByName("*writelog*", false) + 
	All.FindByName("*write_log*", false) + 
	All.FindByName("*write2log*", false) + 
	All.FindByName("*log.print*", false) + 
	All.FindByName("*log_print*", false) + 
	All.FindByName("*printlog*", false) + 
	All.FindByName("*print_log*", false) + 
	All.FindByName("*writetolog*", false);

result = Log + All.FindByMemberAccess("App.LogEvent",false);