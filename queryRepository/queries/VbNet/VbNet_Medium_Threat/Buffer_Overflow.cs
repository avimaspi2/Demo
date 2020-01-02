CxList Inputs = Find_Interactive_Inputs();
CxList Length = All.FindByName("*.Length", false).DataInfluencedBy(Inputs);

CxList DllImport = All.FindByName("*DllImport*", false).FindByType(typeof(CustomAttribute));
CxList ExternalMethod = DllImport.GetFathers().FindByType(typeof(MethodDecl));
ExternalMethod = All.FindAllReferences(ExternalMethod);

CxList ExternalMethodParams = All.GetParameters(ExternalMethod);
ExternalMethodParams = ExternalMethodParams - ExternalMethodParams.InfluencedBy(Length);

CxList Unsafe = All.GetByMethod(All.FindByFieldAttributes(Modifiers.Unsafe));
Unsafe  = Unsafe  - Unsafe.InfluencedBy(Length);

result = (Unsafe + ExternalMethodParams).DataInfluencedBy(Inputs);