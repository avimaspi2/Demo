CxList logs = All.FindByTypes(new string [] {"Log", "Logger"});
// Remove the ThreadLocal because it's a thread safe object
CxList localThread = All.FindByType("ThreadLocal");
// Remove Location.getLocation static field because it is never changed after its initialization
CxList locations = All.FindByMemberAccess("Location.getLocation").GetAncOfType(typeof(FieldDecl));

CxList toRemove = All.NewCxList();
toRemove.Add(logs);
toRemove.Add(localThread);
toRemove.Add(locations);

// Remove false sinks such asTypeRef and GenericTypeRef
CxList no_logs = All - logs;
no_logs -= no_logs.FindByType(typeof(ThisRef));

CxList statics = 
	no_logs.FindAllReferences(no_logs.FindByFieldAttributes(Modifiers.Static) - 
	no_logs.FindByFieldAttributes(Modifiers.Sealed) - 
	no_logs.FindByFieldAttributes(Modifiers.Readonly) -
	locations);
statics -= Find_Methods();

CxList staticDecl = statics.FindByType(typeof(Declarator));
statics -= staticDecl;
 
CxList inputs = Find_Interactive_Inputs();
inputs.Add(All.FindByMemberAccess("ServerRequest.getAttribute"));

CxList threadSafetyIssue = inputs.InfluencingOnAndNotSanitized(statics, staticDecl);

CxList inputInitializers = inputs.GetByAncs(staticDecl);
foreach (CxList initializedInput in inputInitializers)
{
	CxList decl = initializedInput.GetAncOfType(typeof(Declarator));
	threadSafetyIssue.Add(initializedInput.ConcatenateAllTargets(decl));
}

CxList Servlet = All.FindByName("*HttpServletRequest");
CxList ServletClasses = All.GetClass(Servlet);

CxList StaticFields = Find_Field_Decl().FindByFieldAttributes(Checkmarx.Dom.Modifiers.Static);
StaticFields -= toRemove;
StaticFields = All.FindAllReferences(StaticFields) - StaticFields;
StaticFields = StaticFields.GetByAncs(ServletClasses);

// remove init() method of HttpServlet because it's only run once
// in one thread during the application life cycle
CxList initMethods = Find_MethodDecls().FindByShortName("init").GetByAncs(ServletClasses);
StaticFields -= StaticFields.GetByAncs(initMethods);

CxList Locks = All.FindByType(typeof(LockStmt));
StaticFields = StaticFields - StaticFields.GetByAncs(Locks);

CxList nullLiteral = Find_NullLiteral();
nullLiteral -= nullLiteral.FindByRegex("null");
CxList nonNull = All - nullLiteral - StaticFields - StaticFields.GetMembersOfTarget();
StaticFields = All * StaticFields.DataInfluencingOn(nonNull).DataInfluencedBy(nonNull)
	+ StaticFields.GetByAncs(All.FindByType(typeof(PostfixExpr)).GetFathers());

CxList StaticFieldsDef = All.FindDefinition(StaticFields);
StaticFields -= StaticFieldsDef;

result = threadSafetyIssue;
result.Add(StaticFields);