CxList propsDecl = Find_PropertyDecl();
CxList typeRef = Find_TypeRef();
CxList classDecl = Find_ClassDecl();
CxList methodDecls = Find_MethodDecls();
CxList customProperties = Find_CustomAttribute();

CxList publicFiels = propsDecl.FindByFieldAttributes(Modifiers.Public);
//Types no primitive
CxList noPrimitiveTypes = typeRef.FindAllReferences(classDecl);
//get all the no primtive types in public properties
CxList publicNoPrimitiveTypes = noPrimitiveTypes.FindByFathers(publicFiels);
//get class with public fields (doesn't matter if is primitve or not)
CxList classWithPublicFiels = publicFiels.GetAncOfType(typeof(ClassDecl));
//get no primitive types that in definitions has public properties 
CxList typeRefFromClassWithPublicMembers = typeRef.FindAllReferences(classWithPublicFiels);
//intersection between all the references to no primitive type and no primitive type in public
CxList propertiesPublicFromTypeNonPremitive = typeRefFromClassWithPublicMembers * publicNoPrimitiveTypes;
//class with public fiels from no primitive types
CxList classToSearchOnView = propertiesPublicFromTypeNonPremitive.GetAncOfType(typeof(ClassDecl));
//class type pagemodel
CxList classFrontEnd = classDecl.InheritsFrom("PageModel");
//methods OnPost on class pagemodel
CxList methodsOnPostFront = methodDecls.FindByShortNames(new List<string>{"OnPost","OnGet"}).GetByAncs(classFrontEnd);
//parameters of type no primitive 
CxList paramTypeNoPrimitive = typeRef.FindAllReferences(classToSearchOnView).GetAncOfType(typeof(ParamDecl));
//properties of type no primitive 
CxList propertyTypeNoPrimitive = typeRef.FindAllReferences(classToSearchOnView).GetAncOfType(typeof(PropertyDecl));
//parameters on method onPost
//Bind properties
CxList propertyDeclCustomAttribute = customProperties
	.FindByShortName("BindProperty")
	.GetAncOfType(typeof(PropertyDecl));
//intersection between properties Class and Properties no primitive type
result.Add(propertyTypeNoPrimitive * propertyDeclCustomAttribute);
result.Add(paramTypeNoPrimitive.GetParameters(methodsOnPostFront));