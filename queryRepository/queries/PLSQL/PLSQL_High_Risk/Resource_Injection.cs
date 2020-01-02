CxList inputs = Find_Interactive_Inputs();

CxList resource = 
	All.FindByMemberAccess("DBMS_CUBE.BUILD", false) + 
	All.FindByMemberAccess("DBMS_FILE_TRANSFER.COPY_FILE", false) + 
	All.FindByMemberAccess("DBMS_FILE_TRANSFER.GET_FILE", false) + 
	All.FindByMemberAccess("DBMS_FILE_TRANSFER.PUT_FILE", false) + 
	All.FindByMemberAccess("DBMS_SCHEDULER.GET_FILE", false) + 
	All.FindByMemberAccess("DBMS_SCHEDULER.PUT_FILE", false) + 
	All.FindByMemberAccess("DBMS_SCHEDULER.CREATE_PROGRAM", false) + 
	All.FindByMemberAccess("DBMS_SERVICE.CREATE_SERVICE", false) + 
	All.FindByMemberAccess("UTL_TCP.OPEN_CONNECTION", false) +
	All.FindByMemberAccess("UTL_SMTP.OPEN_CONNECTION", false) + 
	All.FindByMemberAccess("WPG_DOCLOAD.DOWNLOAD_FILE", false);


result = inputs.DataInfluencingOn(resource);