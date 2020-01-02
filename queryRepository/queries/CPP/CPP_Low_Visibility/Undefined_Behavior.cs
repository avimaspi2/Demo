//  The Linux Standard Base Specification 2.0.1 for libc places constraints on the arguments to some internal functions[1].If the constraints are not met, the behavior of the functions is not defined.
//  The value 1 must be passed to the first parameter(the version number) of the following file system function:
//					__xmknod
//  The value 2 must be passed to the third parameter(the group argument) of the following wide character string functions:
//				   __wcstod_internal
//                 __wcstof_internal
//                 __wcstol_internal
//                 __wcstold_internal
//                 __wcstoul_internal
//	The value 3 must be passed as the first parameter (the version number) of the following file system functions:
//			        __xstat
//					__lxstat
//					__fxstat
//					__xstat64
//					__lxstat64
//					__fxstat64
// Based on: http://lab.gsi.dit.upm.es/semanticwiki/index.php/Undefined_Behavior
//           http://chadmoore.us/source/gnu/glibc/glibc-2.11.1/include/wchar.h

CxList methods = Find_Methods();
CxList valueOne = methods.FindByShortName("__xmknod");
CxList res = All.GetParameters(valueOne, 0).FindByType(typeof(IntegerLiteral)).FindByShortName("1");
result = valueOne - res.GetAncOfType(typeof(MethodInvokeExpr));

CxList valueTwo = methods.FindByShortName("__wcstod_internal") + 
	methods.FindByShortName("__wcstof_internal") + 
	methods.FindByShortName("__wcstol_internal") + 
	methods.FindByShortName("__wcstold_internal") + 
	methods.FindByShortName("__wcstoul_internal");

res = All.GetParameters(valueTwo, 2).FindByType(typeof(IntegerLiteral)).FindByShortName("2");
result.Add(valueTwo - res.GetAncOfType(typeof(MethodInvokeExpr)));

CxList valueThree = methods.FindByShortName("__xstat") + 
	methods.FindByShortName("__lxstat") + 
	methods.FindByShortName("__fxstat") + 
	methods.FindByShortName("__xstat64") + 
	methods.FindByShortName("__lxstat64") + 
	methods.FindByShortName("__fxstat64");

res = All.GetParameters(valueThree, 0).FindByType(typeof(IntegerLiteral)).FindByShortName("3");
result.Add(valueThree - res.GetAncOfType(typeof(MethodInvokeExpr)));