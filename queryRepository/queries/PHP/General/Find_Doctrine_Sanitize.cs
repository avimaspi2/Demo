CxList DB_IN = Find_Doctrine_DB_In();
result.Add(DB_IN.FindByShortName("setParameter*"));
result.Add(DB_IN.FindByShortName("persist"));

/*
CxList doctrine = All.FindByShortName("*Doctrine*");
doctrine -= doctrine.FindByShortName("*RawSql");

CxList whereStmt=DB_IN.FindByShortName("*where*", false);
CxList influencedByDoctrine = whereStmt.DataInfluencedBy(doctrine);
CxList firstParam = All.GetParameters(influencedByDoctrine, 0);
CxList AllParamsButFirst = All.GetParameters(influencedByDoctrine);
result.Add(AllParamsButFirst - firstParam);
*/

//add all ORM finds
CxList methods = Find_Methods();

//find short query repository->findAll(condition)
CxList unknownRef = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(MemberAccess));

CxList repository = unknownRef.FindByShortName("*repository", false) +
	methods.FindByShortNames(new List<String>(){ "getRepository", "getEntityManager" }) +
	unknownRef.FindByShortName("entityManager") +
	All.FindByShortName("Doctrine*");

CxList find = repository.GetMembersOfTarget();
result.Add(find.FindByShortNames(new List<String>(){ "find", "findOneBy*", "findAll", "findBy*" }));