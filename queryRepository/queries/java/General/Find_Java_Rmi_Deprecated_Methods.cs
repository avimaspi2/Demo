CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.rmi.dgc.VMID
	"VMID.isUnique",
	// java.rmi.registry.RegistryHandler
	"RegistryHandler.registryImpl",
	"RegistryHandler.registryStub",
	// java.rmi.server.LoaderHandler
	"LoaderHandler.getSecurityContext",
	"LoaderHandler.loadClass",
	// java.rmi.server.LogStream
	"LogStream.getDefaultStream",
	"LogStream.getOutputStream",
	"LogStream.log",
	"LogStream.parseLevel",
	"LogStream.setDefaultStream",
	"LogStream.setOutputStream",
	"LogStream.toString",
	"LogStream.write",
	// java.rmi.server.Operation
	"Operation.getOperation",
	"Operation.toString",
	// java.rmi.server.RemoteCall
	"RemoteCall.done",
	"RemoteCall.executeCall",
	"RemoteCall.getInputStream",
	"RemoteCall.getOutputStream",
	"RemoteCall.getResultStream",
	"RemoteCall.releaseInputStream",
	"RemoteCall.releaseOutputStream",
	// java.rmi.server.RemoteRef
	"RemoteRef.done",
	"RemoteRef.newCall",
	// java.rmi.server.RemoteStub
	"RemoteStub.setRef"}));

// java.rmi.server.RemoteRef.invoke(RemoteCall) is deprecated (was replaced with a 4-args equivalent)
CxList invokeInvokes = methods.FindByMemberAccess("RemoteRef.invoke");
CxList viewToModel_2nd_Params = All.GetParameters(invokeInvokes, 1);
result.Add(invokeInvokes - invokeInvokes.FindByParameters(viewToModel_2nd_Params));

// java.rmi.server.RMIClassLoader
result.Add(methods.FindByMemberAccess("RMIClassLoader.getSecurityContext"));

// java.rmi.server.RMIClassLoader.loadClass(string) is deprecated
CxList RMI_loadClass = methods.FindByMemberAccess("RMIClassLoader.loadClass");
CxList RMI_loadClass_2nd_Param = All.GetParameters(RMI_loadClass, 1); 
result.Add(RMI_loadClass - RMI_loadClass.FindByParameters(RMI_loadClass_2nd_Param));	

// java.rmi.server.Skeleton
result.Add(methods.FindByMemberAccess("Skeleton.dispatch"));
result.Add(methods.FindByMemberAccess("Skeleton.getOperations"));