//Finds All query references
CxList session = All.NewCxList();
session.Add(All.FindByMemberAccess("request.getSession"));
session.Add(All.FindByName("*request.getSession"));
session.Add(All.FindByName("*Request.getSession"));
session.Add(All.FindByMemberAccess("HibernateUtil.currentSession"));
session.Add(All.FindByType("Session"));
session.Add(Find_Element_In_Collections_Of_Type("Session"));
session.Add(All.FindAllReferences(All.FindByReturnType("Session")));

//Query Interfaces
string[] types = new string[] {
	"Query",
	"QueryImpl",
	"TypedQuery",
	"SQLQueryImpl",
	"SQLQuery",
	"AbstractQueryImpl",
	"EJBQuery",
	"JpaQuery",
	"CollectionFilterImpl",
	"StaticQuery"
	};

CxList qTypes = All.FindByTypes(types);
qTypes.Add(Find_Element_In_Collections_Of_Type(types)); 

//classes that inherit from Query Interfaces
CxList qTypeInherit = All.NewCxList();
qTypeInherit.Add(All.InheritsFrom("Query"));
qTypeInherit.Add(All.InheritsFrom("QueryImpl"));
qTypeInherit.Add(All.InheritsFrom("TypedQuery"));
qTypeInherit.Add(All.InheritsFrom("SQLQueryImpl"));
qTypeInherit.Add(All.InheritsFrom("SQLQuery"));
qTypeInherit.Add(All.InheritsFrom("AbstractQueryImpl"));
qTypeInherit.Add(All.InheritsFrom("EJBQuery"));
qTypeInherit.Add(All.InheritsFrom("JpaQuery"));
qTypeInherit.Add(All.InheritsFrom("CollectionFilterImpl"));

qTypeInherit = All.FindAllMembers(qTypeInherit);
qTypes.Add(qTypeInherit);

CxList qDefinitions = qTypes - qTypes.FindByType(typeof(TypeRef));	//Query types objects
CxList qTypeMem = qTypes.GetMembersOfTarget();


//set* functions
CxList setQueryMet = qTypeMem.FindByShortName("set*");
setQueryMet -= setQueryMet.FindByShortName("setOptional*");

CxList memberAccessSessionCreate = All.FindByMemberAccess("session.create*");
result = All.NewCxList();
result.Add(setQueryMet);
result.Add(memberAccessSessionCreate.FindByMemberAccess("session.createQuery"));
result.Add(memberAccessSessionCreate.FindByMemberAccess("session.createSQLQuery"));
result.Add(session.GetMembersOfTarget().FindByShortNames(new List<string> {
		"createQuery",
		"createSQLQuery"}));
result.Add(qDefinitions);

int maxIterationNum = 50;				//limits iterations in while loop
CxList tempResult = result.GetMembersOfTarget();

while (tempResult.Count > 0 && maxIterationNum-- > 0){
	tempResult = tempResult.FindByShortName("set*"); 
tempResult -= tempResult.FindByShortName("setOptional*");
	
result.Add(tempResult);
tempResult = result.GetMembersOfTarget();
}