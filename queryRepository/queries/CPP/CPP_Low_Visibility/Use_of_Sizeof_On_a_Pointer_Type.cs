//Get sizeof occurrences
CxList sizeOfMethods = Find_Methods().FindByShortName("sizeof");
CxList relevantTypes = Find_Unknown_References();
relevantTypes.Add(Find_MemberAccesses());
CxList unaryOps = Find_Unarys();
CxList pointers = Find_Pointers();

//Get the parameters of the relevant sizeof occurrences
CxList parametersOfSizeof = All.GetParameters(sizeOfMethods); 
parametersOfSizeof -= Find_Param();

//remove correct uses of sizeof, like malloc(sizeof( * p)),
//where the size of the data structure is returned 
parametersOfSizeof -= (parametersOfSizeof * unaryOps).FindByShortName("Pointer");


//Find pointer declarations of sizeof parameters
//Find arrays received by a function as a parameter, since they will return size of the pointer
CxList pointerDeclarations = Find_Pointer_Decl(); 
pointerDeclarations.Add(pointers.GetByAncs(Find_ParamDecl()));
pointerDeclarations.Add(Find_Array_Declaration() * Find_ParamDecl());
CxList relevantPointerDecls = pointerDeclarations.FindDefinition(parametersOfSizeof);


//Sizeof of any pointer variable will return the size of the pointer to the data structure
//cases like malloc(sizeof(*p));
CxList relevantParameters = parametersOfSizeof * relevantTypes;
relevantParameters = relevantParameters.FindAllReferences(relevantPointerDecls);

//Sizeof of an address 
//cases like "void someFunc(LOGFONTA &lf){
//	               malloc(sizeof(lf); }"
CxList useOfAddress = (parametersOfSizeof * unaryOps).FindByShortName("Address");
CxList varNames = relevantTypes.FindByFathers(useOfAddress);
relevantParameters.Add(varNames);

//Sizeof of PointerTypeRefs
//cases like malloc(sizeof(struct two_fields *));
CxList typeOfExprs = parametersOfSizeof.FindByType(typeof(TypeOfExpr)); 
CxList pointerTypeRefsInTypeOf = pointers.GetFathers() * typeOfExprs;
relevantParameters.Add(pointerTypeRefsInTypeOf);

//Remove pointers to built-in types (not variables) since it's usually intended behaviour
CxList builtinTypes = All.FindByTypes(new string[]{"int", "long", "short", "float", "double", "bool", "char", "wchar_t"});
CxList sizeofPointerArgs = All.GetByAncs(parametersOfSizeof).FindByType(typeof(PointerTypeRef));
CxList sizeofBuiltinPointerTypeArgs = sizeofPointerArgs * builtinTypes;
relevantParameters -= sizeofBuiltinPointerTypeArgs.GetFathers().FindByType(typeof(TypeOfExpr));

foreach (CxList sp in relevantParameters)
{
	result.Add(relevantPointerDecls.FindDefinition(sp).Concatenate(sizeOfMethods.FindByParameters(sp)));
}