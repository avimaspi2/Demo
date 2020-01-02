//Methods from java.lang.SecurityManager throwing Exceptions

string[] methodNames = new string[] {
	"SecurityManager.checkAccept",
	"SecurityManager.checkAccess",
	"SecurityManager.checkConnect",
	"SecurityManager.checkCreateClassLoader",
	"SecurityManager.checkDelete",
	"SecurityManager.checkExec",
	"SecurityManager.checkExit?",
	"SecurityManager.checkLink",
	"SecurityManager.checkListen",
	"SecurityManager.checkMemberAccess",
	"SecurityManager.checkMulticast",
	"SecurityManager.checkPackageAccess",
	"SecurityManager.checkPackageDefinition",
	"SecurityManager.checkPermission",
	"SecurityManager.checkPrintJobAccess",
	"SecurityManager.checkPropertiesAccess",
	"SecurityManager.checkPropertyAccess?",
	"SecurityManager.checkRead?",
	"SecurityManager.checkSecurityAccess",
	"SecurityManager.checkSetFactory",
	"SecurityManager.checkWrite"
	};

result = All.FindByMemberAccesses(methodNames, false);