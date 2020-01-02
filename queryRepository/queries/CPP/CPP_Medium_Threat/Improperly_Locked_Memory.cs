CxList methods = Find_Methods();
CxList badForSecurity = 
	  methods.FindByShortName("VirtualLock")
	+ methods.FindByShortName("mlock");
CxList personal = Find_Personal_Info();

result = badForSecurity.DataInfluencedBy(personal);