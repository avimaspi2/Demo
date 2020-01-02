result = 
	All.FindByMemberAccess("DBMS_RANDOM.NORMAL", false) + 
	All.FindByMemberAccess("DBMS_RANDOM.STRING", false) +
	All.FindByMemberAccess("DBMS_RANDOM.RANDOM", false) +
	All.FindByMemberAccess("DBMS_RANDOM.VALUE", false);