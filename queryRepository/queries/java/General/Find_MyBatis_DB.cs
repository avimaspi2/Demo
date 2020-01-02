CxList methods = Find_Methods();

CxList unknowReferences = Find_UnknownReference();

CxList mappers = methods.FindByMemberAccess("SqlSession.getMapper");

CxList myBatisMethods = All.NewCxList();
myBatisMethods.Add(mappers);
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.selectOne"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.insert"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.select"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.delete"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.selectCursor"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.selectList"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.selectMap"));
myBatisMethods.Add(methods.FindByMemberAccess("SqlSession.update"));


CxList mappersDecls = unknowReferences.FindAllReferences((mappers * myBatisMethods).GetAncOfType(typeof(Declarator)));

CxList mappersUknowRef = unknowReferences.FindByFathers((mappers * myBatisMethods).GetFathers()).FindByAssignmentSide(CxList.AssignmentSide.Left);

mappersUknowRef = unknowReferences.FindAllReferences(mappersUknowRef).GetMembersOfTarget();

mappersUknowRef.Add(mappersDecls.GetMembersOfTarget());

List <string> defaultObjectsMethods = new List<string>{
		"clone",
		"equals",
		"finalize",
		"getClass",
		"hashCode",
		"notify",
		"notifyAll",
		"wait"
		};

mappersUknowRef -= mappersUknowRef.FindByShortNames(defaultObjectsMethods);

myBatisMethods.Add(mappersUknowRef);
myBatisMethods -= mappers;

result = myBatisMethods;