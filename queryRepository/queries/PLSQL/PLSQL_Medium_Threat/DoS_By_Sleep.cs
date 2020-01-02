CxList inputs = Find_Inputs();
CxList sleep = 
	All.FindByMemberAccess("dbms_backup_restore.sleep", false) + 
	All.FindByMemberAccess("dbms_drs.sleep", false) + 
	All.FindByMemberAccess("dbms_lock.sleep", false) + 
	All.FindByMemberAccess("user_lock.sleep", false); 
	

result = sleep.DataInfluencedBy(inputs);