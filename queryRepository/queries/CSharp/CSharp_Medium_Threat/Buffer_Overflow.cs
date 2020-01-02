CxList Inputs = Find_Interactive_Inputs();
CxList Length = All.FindByName("*.Length").DataInfluencedBy(Inputs);
CxList DllImport = All.FindByName("*DllImport*").FindByType(typeof(CustomAttribute));
CxList ExternalMethod = DllImport.GetFathers().GetFathers().FindByType(typeof(MethodDecl));
ExternalMethod = All.FindAllReferences(ExternalMethod);
CxList ExternalMethodParams = All.GetParameters(ExternalMethod);
ExternalMethodParams = ExternalMethodParams - ExternalMethodParams.InfluencedBy(Length);

CxList Unsafe = All.GetByMethod(All.FindByFieldAttributes(Modifiers.Unsafe));
Unsafe = Unsafe - Unsafe.InfluencedBy(Length);

//add all integers to buffer overflow sanitizers
CxList rs = All.FindByType(typeof(RankSpecifier));
CxList declarator = All.FindByType(typeof(Declarator));
CxList allRefs = All.FindByType(typeof(UnknownReference)) + declarator;
CxList type = rs.GetFathers().FindByType(typeof(TypeRef));
CxList pointers = declarator.FindByRegex(@"[^(\s]\s*\*\s*\w", CxList.CxRegexOptions.None, RegexOptions.None, null);
pointers = All.GetByAncs(pointers).FindByType(typeof(Declarator)); 
pointers = Unsafe.FindAllReferences(pointers);
CxList ints = Find_Integers() + All.FindByTypes(new String[] {"char", "System.Char"}) - pointers; 
CxList arrays = allRefs.FindAllReferences(ints * declarator.GetByAncs(type.GetFathers())); 
CxList sanitize = ints - arrays;


result = (pointers + ExternalMethodParams).InfluencedByAndNotSanitized(Inputs, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);