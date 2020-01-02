CxList methods = Find_Methods();
CxList unknownRef = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(MemberAccess));

//find short query repository->findAll(condition)
CxList repository = unknownRef.FindByShortName("*repository", false) +
	methods.FindByShortNames(new List<String>(){ "getRepository", "getEntityManager" }) + 
	unknownRef.FindByShortName("entityManager") + 
	All.FindByShortName("Doctrine*");

CxList find = repository.GetMembersOfTarget();

result.Add(find.FindByShortNames(new List<string> {"find", "findOneBy*", "findAll", "findBy*"}));
/*
result.Add(find.FindByShortName("find") +
	find.FindByShortName("findOneBy*") +
	find.FindByShortName("findAll") + 
	find.FindByShortName("findBy*"));
*/
//find query result that indluenced by createQuery or createNativeQuery
CxList resultType = methods.FindByShortNames(new List<string> {"getResult", "getSingleResult", 
		"getArrayResult", "getScalarResult", "getSingleScalarResult","getOneOrNullResult"});
/*
CxList resultType = methods.FindByShortName("getResult") +
	methods.FindByShortName("getSingleResult") +
	methods.FindByShortName("getArrayResult") +
	methods.FindByShortName("getScalarResult") +
	methods.FindByShortName("getSingleScalarResult")+
	methods.FindByShortName("getOneOrNullResult");
*/

CxList query = resultType.GetTargetOfMembers();
query *= query.DataInfluencedBy(methods.FindByShortName("createQuery*") +
	methods.FindByShortName("createNativeQuery"));

result.Add(query.GetMembersOfTarget());


CxList queryExec = methods.FindByShortName("execute").GetTargetOfMembers();
CxList getQuery = methods.FindByShortName("createQueryBuilder") +
	(unknownRef + All.FindByType(typeof(ObjectCreateExpr))).FindByShortName("Doctrine*");
CxList execute = ((queryExec * queryExec.DataInfluencedBy(getQuery)).GetMembersOfTarget());
result.Add(execute);