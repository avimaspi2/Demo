CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.lang.Character
	"Character.isJavaLetter",
	"Character.isJavaLetterOrDigit",
	"Character.isSpace",
	// java.lang.Runtime
	"Runtime.getLocalizedInputStream",
	"Runtime.getLocalizedOutputStream",
	"Runtime.runFinalizersOnExit",
	"Runtime.traceInstructions",
	"Runtime.traceMethodCalls",
	// java.lang.System
	"System.runFinalizersOnExit",
	// java.lang.Thread
	"Thread.countStackFrames",
	"Thread.destroy",
	"Thread.resume",
	"Thread.stop",
	"Thread.suspend",
	// java.lang.ThreadGroup
	"ThreadGroup.allowThreadSuspension",
	"ThreadGroup.resume",
	"ThreadGroup.stop",
	"ThreadGroup.suspend"}));

// java.lang.SecurityManager
CxList temp = methods.FindByMemberAccess("SecurityManager.*");
result.Add(temp.FindByShortNames(new List<string>{
		"checkAwtEventQueueAccess",
		"checkMemberAccess",
		"checkSystemClipboardAccess",
		"checkTopLevelWindow",
		"classDepth",
		"classLoaderDepth",
		"currentClassLoader",
		"currentLoadedClass",
		"getInCheck",
		"inClass",
		"inClassLoader"}));

// java.lang.ClassLoader.defineClass(byte[], int, int) is deprecated
CxList defineClass = methods.FindByMemberAccess("ClassLoader.defineClass");
CxList defineClass_1st_Param = All.GetParameters(defineClass, 0).FindByType("byte"); 
result.Add(defineClass.FindByParameters(defineClass_1st_Param));

// java.lang.SecurityManager.checkMultiCast(InitAddress, Byte) is deprecated (was replaced with a 1-arg equivalent)
CxList checkMultiCast = methods.FindByMemberAccess("SecurityManager.checkMulticast");
CxList checkMultiCast_2nd_Params = All.GetParameters(checkMultiCast, 1);
result.Add(checkMultiCast.FindByParameters(checkMultiCast_2nd_Params));

// java.lang.String: Only the "getBytes" method with 4 arguments is deprecated
CxList getBytes = methods.FindByMemberAccess("String.getBytes");
// Go over all "getBytes" methods and add just the one that is deprecated
CxList getBytesParams = All.GetParameters(getBytes, 3).FindByType(typeof(Param)); 
result.Add(getBytesParams.GetAncOfType(typeof(MethodInvokeExpr)));

CxList baseRefs = Find_BaseRef();
CxList thisRefs = Find_ThisRef();
CxList refs = All.NewCxList();
refs.Add(baseRefs.GetMembersOfTarget(), thisRefs.GetMembersOfTarget(), Find_UnknownReference());

// java.lang.SecurityManager.inCheck protected field: accessible only from inherited class
CxList inCheck = refs.FindByShortName("inCheck");
CxList classes = Find_Classes().GetClass(inCheck);
CxList securityManagerCls = classes.InheritsFrom("SecurityManager");
result.Add(inCheck.GetByClass(securityManagerCls));