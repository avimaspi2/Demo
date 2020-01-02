//Finds All query references

CxList session = All.FindByMemberAccess("request.getSession") +
	All.FindByName("*request.getSession") +
	All.FindByName("*Request.getSession") +
	All.FindByMemberAccess("HibernateUtil.currentSession") +
	All.FindByType("Session") +
	All.FindAllReferences(All.FindByReturnType("Session"));

//Query Interfaces
CxList qTypes = All.FindByType("Query") +
	All.FindByType("QueryImpl") +
	All.FindByType("TypedQuery") + 
	All.FindByType("SQLQueryImpl") +
	All.FindByType("SQLQuery") +
	All.FindByType("AbstractQueryImpl") +
	All.FindByType("EJBQuery") +
	All.FindByType("JpaQuery") +
	All.FindByType("CollectionFilterImpl");
	
//classes that inherit from Query Interfaces
CxList qTypeInherit = All.InheritsFrom("Query") +
	All.InheritsFrom("QueryImpl") +
	All.InheritsFrom("TypedQuery") +
	All.InheritsFrom("SQLQueryImpl") +
	All.InheritsFrom("SQLQuery") +
	All.InheritsFrom("AbstractQueryImpl") +
	All.InheritsFrom("EJBQuery") +
	All.InheritsFrom("JpaQuery") +
	All.InheritsFrom("CollectionFilterImpl");
qTypeInherit = All.FindAllMembers(qTypeInherit);
qTypes.Add(qTypeInherit);

CxList qDefinitions = qTypes - qTypes.FindByType(typeof(TypeRef));	//Query types objects
CxList qTypeMem = qTypes.GetMembersOfTarget();

//set* functions
CxList setQueryMet = qTypeMem.FindByShortName("set*");
setQueryMet -= setQueryMet.FindByShortName("setOptional*");

result = 
	setQueryMet + 
	All.FindByMemberAccess("session.createQuery") +
	session.GetMembersOfTarget().FindByShortName("createQuery") +
	All.FindByMemberAccess("session.createSQLQuery") +
	session.GetMembersOfTarget().FindByShortName("createSQLQuery") +
	qDefinitions;

int maxIterationNum = 50;				//limits iterations in while loop
CxList tempResult = result.GetMembersOfTarget();

/*
finds concatenate functions of query type, for example:
	 	Query query = em.createQuery("select a.id, a.name from Applicant a");
		return query.setLong("ABC", 5).setHint().setCalendar(i, obj);
*/
while (tempResult.Count > 0 && maxIterationNum-- > 0){
	tempResult = tempResult.FindByShortName("set*"); 
tempResult -= tempResult.FindByShortName("setOptional*");
tempResult -= result;
result.Add(tempResult);
tempResult = result.GetMembersOfTarget();
}