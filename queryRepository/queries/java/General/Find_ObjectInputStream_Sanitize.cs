/*
Find parameters of object creation of classes that follow the secure pattern
https://github.com/OWASP/CheatSheetSeries/blob/master/cheatsheets/Deserialization_Cheat_Sheet.md#harden-your-own-javaioobjectinputstream
*/

CxList classDecls = Find_ClassDecl();
CxList constructorDecls = Find_ConstructorDecl();
CxList methodDecls = Find_MethodDecls();
CxList conditions = Find_Conditions();
CxList paramDecl = Find_ParamDecl();
CxList unkRefs = Find_UnknownReference();

CxList extendedClasses = classDecls.InheritsFrom("ObjectInputStream");
CxList goodExtendedClasses = All.NewCxList();

CxList resolveClass = methodDecls.FindByShortName("resolveClass");
resolveClass = resolveClass.GetByAncs(extendedClasses);
CxList resolveClassParam = paramDecl.GetParameters(resolveClass);
CxList paramRefs = unkRefs.FindAllReferences(resolveClassParam);
CxList ifConditions = conditions.GetByAncs(resolveClass);
CxList paramInConditions = paramRefs.GetByAncs(ifConditions);
CxList nameOfClassChecked = paramInConditions.GetMembersOfTarget().FindByShortName("getName");
goodExtendedClasses.Add(nameOfClassChecked.GetAncOfType(typeof(ClassDecl)));

CxList classRefs = All.FindAllReferences(goodExtendedClasses);
CxList extendedObjCreate = classRefs.GetAncOfType(typeof(ObjectCreateExpr));

result = All.GetParameters(extendedObjCreate);