// Unreleased_Resource_Synchronization
// -----------------------------------

// There are some Linux methods that can reserve system resources, and if this resources
// will not be released it may cause to resources leak 
// http://www.yolinux.com/TUTORIALS/LinuxTutorialPosixThreads.html
// http://publib.boulder.ibm.com/infocenter/iseries/v7r1m0/index.jsp?topic=%2Fapis%2Fusers_75.htm
CxList methods = Find_Methods();

CxList resourceInit = methods.FindByShortName("pthread_cond_init")      + 
					  methods.FindByShortName("pthread_mutex_lock")     + 
					  methods.FindByShortName("pthread_condattr_init")  + 
	    			  methods.FindByShortName("pthread_attr_init")      + 
					  methods.FindByShortName("pthread_mutexattr_init") + 
					  methods.FindByShortName("pthread_mutex_init");
CxList reserveTokens = All.GetParameters(resourceInit, 0);
CxList reserveTokensVar = All.GetByAncs(reserveTokens).FindByType(typeof(UnknownReference));

CxList resourceRelease = methods.FindByShortName("pthread_cond_destroy") + 
						 methods.FindByShortName("pthread_mutex_unlock")+
						 methods.FindByShortName("pthread_condattr_destroy")+
						 methods.FindByShortName("pthread_attr_destroy")+
	                     methods.FindByShortName("pthread_mutexattr_destroy")+
	                     methods.FindByShortName("pthread_mutex_destroy")	;
CxList releaseTokens = All.GetParameters(resourceRelease, 0);
CxList releaseTokensVar = All.GetByAncs(releaseTokens).FindByType(typeof(UnknownReference));

result = reserveTokensVar - reserveTokensVar.DataInfluencingOn(releaseTokensVar);