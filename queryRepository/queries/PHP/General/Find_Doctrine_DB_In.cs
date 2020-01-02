CxList methods = Find_Methods();

//find short query repository->findAll(condition)
CxList unknownRef = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(MemberAccess));

CxList repository = unknownRef.FindByShortNames(new List<String>(){ "*repository", "entityManager" }, false);
repository.Add(methods.FindByShortNames(new List<String>(){ "getRepository", "getEntityManager" }));
repository.Add(All.FindByShortName("Doctrine*"));

CxList find = repository.GetMembersOfTarget();

result.Add(find.FindByShortNames(new List<string> {"find", "findOneBy*", "findAll", "findBy*"}));
/*
result.Add(
	find.FindByShortName("find") + 
	find.FindByShortName("findOneBy*") +
	find.FindByShortName("findAll") +
	find.FindByShortName("findBy*"));
*/
CxList queryExec = methods.FindByShortName("execute").GetTargetOfMembers();
CxList getQuery = methods.FindByShortName("createQueryBuilder");
getQuery.Add((unknownRef + All.FindByType(typeof(ObjectCreateExpr))).FindByShortName("Doctrine*"));
CxList execute = ((queryExec * queryExec.DataInfluencedBy(getQuery)).GetMembersOfTarget());
result.Add(execute);

//find query result that indluenced by createQuery or createNativeQuery
CxList resultType = methods.FindByShortNames(new List<string> {"getResult", "getSingleResult", 
		"getArrayResult", "getScalarResult", "getSingleScalarResult","getOneOrNullResult"});
/*
CxList resultType = 
	methods.FindByShortName("getResult") +
	methods.FindByShortName("getSingleResult") +
	methods.FindByShortName("getArrayResult") +
	methods.FindByShortName("getScalarResult") +
	methods.FindByShortName("getSingleScalarResult") +
	methods.FindByShortName("getOneOrNullResult");
*/

CxList qb = methods.FindByShortName("createQueryBuilder");
CxList query = resultType.GetTargetOfMembers();
query *= query.DataInfluencedBy(methods.FindByShortName("createQuery*") +
	methods.FindByShortName("createNativeQuery") + qb);

result.Add(query.GetMembersOfTarget());

CxList executingSQL = All.NewCxList();

CxList Doctrine = (unknownRef + All.FindByType(typeof(ObjectCreateExpr))).FindByShortName("Doctrine*");
Doctrine = (methods.FindByShortName("createQuery") +
	methods.FindByShortName("create")).DataInfluencedBy(Doctrine) +
	Doctrine.FindByShortName("*_RawSql");


CxList docAndqb = Doctrine + qb;
CxList ae = docAndqb.GetAncOfType(typeof(AssignExpr));
CxList qbRef = unknownRef.GetByAncs(ae).FindByAssignmentSide(CxList.AssignmentSide.Left);
qbRef = unknownRef.FindByShortName(qbRef).FindAllReferences(qbRef);


CxList iteratingQB = docAndqb + qbRef;

foreach(CxList iterQb in iteratingQB)
{	int i = 0;
	CxList iter = iterQb;
	while(i++ < 100)
	{		
		CxList temp = iter.GetMembersOfTarget();	
		if(temp.FindByShortName(resultType).Count > 0){
			break;
		}
		if(temp.Count > 0)
		{	
			executingSQL.Add(temp);						
			iter = temp;
		}else{
			break;
		}		
	}
}


result.Add(executingSQL);

CxList persist = Find_Methods().FindByShortName("persist");
result.Add(persist);