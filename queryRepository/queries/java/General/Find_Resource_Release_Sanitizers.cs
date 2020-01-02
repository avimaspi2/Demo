/*
From Java 9 and on (and effectively on Java 12 after deprecation of finalize 
methods in FileInput/Output and alike), classes java.lang.ref.PhantomReference 
and java.lang.ref.Cleaner are used to perform a reliable management of 
resources' references.
*/

CxList objectCreates = Find_Object_Create();
CxList relevantRefs = Find_UnknownReference();
CxList thisRefs = All.FindByType(typeof(ThisRef));

relevantRefs.Add(thisRefs);
relevantRefs.Add(Find_Declarators());

CxList parameters = Find_Params();
CxList classDecls = Find_ClassDecl();

CxList methods = Find_Methods();

CxList relevantParams = All.NewCxList();
CxList sanitizers = All.NewCxList();

//External use of PhantomReference, for cases like: new PhantomReference(obj1,obj2).
//In this case, obj1 is a sanitized resource.
CxList allPhantomReferenceConstructors = objectCreates.FindByShortName("PhantomReference");
CxList firstParamsOfPhantomReference = relevantRefs.GetByAncs(parameters.GetParameters(allPhantomReferenceConstructors, 0));
relevantParams.Add(firstParamsOfPhantomReference);

//External use of Cleaner, for cases like:
//Cleaner x = Cleaner.create();
//x.register(obj1, act1);
//In this case, obj1 is a sanitized resource
CxList allCleanerObjects = methods.FindByMemberAccess("Cleaner.create");
allCleanerObjects.Add(relevantRefs.FindAllReferences(allCleanerObjects.GetAssignee()));
CxList allCleanerRegiters = allCleanerObjects.GetMembersOfTarget().FindByShortName("register");
CxList firstParamsOfCleanerRegisters = relevantRefs.GetByAncs(parameters.GetParameters(allCleanerRegiters, 0));
relevantParams.Add(firstParamsOfCleanerRegisters);


//Internal use of PhantomReference (via extension)
//Simplified as 
// - any object of a class extending PhantomReference will be a sanitizer
// - any object passed in the constructor such class will be a sanitizer
CxList subclassesOfPhantomReference = classDecls.InheritsFrom("PhantomReference");
CxList allSubPhantomReferenceConstructors = objectCreates.FindByShortName(subclassesOfPhantomReference);
CxList paramsOfSubPhantomReference = relevantRefs.GetByAncs(parameters.GetParameters(allSubPhantomReferenceConstructors));
relevantParams.Add(paramsOfSubPhantomReference);

//Internal use of Cleaner, for cases like:
//Cleaner x = Cleaner.create();
//x.register(this, act1);
//In this case, all instances of the classes where this occurs are sanitized resources
CxList relevantThisParams = relevantParams * thisRefs;
CxList classesOfThisRefs = relevantThisParams.GetAncOfType(typeof(ClassDecl));
CxList cleanObjCreates = objectCreates.FindByShortName(classesOfThisRefs);
cleanObjCreates.Add(relevantRefs.FindAllReferences(cleanObjCreates.GetAssignee()));



result = relevantRefs.FindAllReferences(relevantParams);
result.Add(cleanObjCreates);