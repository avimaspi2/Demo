result = All.NewCxList();
result.Add(Find_CORBA_Deprecated_Methods());
result.Add(Find_Java_Awt_Deprecated_Methods());
result.Add(Find_Java_IO_Deprecated_Methods());
result.Add(Find_Java_Lang_Deprecated_Methods());
result.Add(Find_Java_Net_Deprecated_Methods());
result.Add(Find_Java_Rmi_Deprecated_Methods());
result.Add(Find_Java_Security_Deprecated_Methods());
result.Add(Find_Java_Sql_Deprecated_Methods());
result.Add(Find_Java_Util_Deprecated_Methods());
result.Add(Find_Javax_Swing_Deprecated_Methods());

CxList methods = Find_Methods();
result.Add(methods.FindByMemberAccesses(new string[]{
	// javax.servlet.ServletRequest
	"ServletRequest.getRealPath",
	// javax.servlet.ServletRequestWrapper
	"ServletRequestWrapper.getRealPath",
	// javax.servlet.http.HttpServletRequest
	"HttpServletRequest.getRealPath",
	// javax.servlet.http.HttpServletRequestWrapper
	"HttpServletRequestWrapper.getRealPath",
	// org.omg.PortableServer.DynamicImplementation
	"DynamicImplementation._ids",
	"DynamicImplementation.invoke",
	// org.owasp.encoder.Encode
	"Encode.forUri"}));

// Add javax.management.snmp deprecated properties
CxList inputs = Find_Inputs();
CxList stringLiterals = Find_String_Literal();
CxList systemGetProperty = inputs.FindByMemberAccess("System.getProperty");
CxList snmpProperties = stringLiterals.FindByShortNames(new List<string>{
	"com.sun.management.snmp.trap",
	"com.sun.management.snmp.interface",
	"com.sun.management.snmp.acl",
	"com.sun.management.snmp.acl.file"});
result.Add(snmpProperties.GetParameters(systemGetProperty));

// Add methods that are marked with the @Deprecated annotation
CxList deprecatedAnnotation = Find_CustomAttribute().FindByShortName("Deprecated");
CxList deprecatedAnnotationMethods = deprecatedAnnotation.GetAncOfType(typeof(MethodDecl));
result.Add(methods.FindAllReferences(deprecatedAnnotationMethods));