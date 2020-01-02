// SQL - sanitize
 
CxList methods = Find_Methods();

CxList hibernate = Find_Hibernate_DB();

CxList hibernateParam_All = 
	hibernate.FindByShortName("get") + 
	hibernate.FindByShortName("load") + 
	hibernate.FindByShortName("findByCriteria") +
	hibernate.FindByShortName("findByExample") +
	hibernate.FindByShortName("findByNamedQuery*");

CxList bulkUpdate = All.FindByParameters(hibernate).FindByShortName("bulkUpdate");
CxList hibernateParameters = All.GetParameters(bulkUpdate, 1);

CxList hibernateParam_NotFirst = 
	hibernate.FindByShortName("find*") + 
	hibernate.FindByShortName("iterate");

hibernateParameters.Add(All.GetParameters(hibernateParam_NotFirst));
hibernateParameters -= hibernateParameters.GetParameters(hibernateParam_NotFirst, 0);
hibernateParameters.Add(All.GetParameters(hibernateParam_All));

CxList criteriaAdd = All.NewCxList();
CxList Criteria = 
	All.FindByShortName("*Criteria") + All.FindByType("*Criteria") +
	All.FindByShortName("Subcriteria") + All.FindByType("Subcriteria") +
	All.FindByShortName("CriteriaImpl") + All.FindByType("CriteriaImpl") +
	All.FindByType("CriteriaSpecification") +
	All.FindByShortName("Projections") + All.FindByType("Projections");

int count = 25;
while (Criteria.Count > 0 && count-- > 0)
{
	criteriaAdd.Add(Criteria.FindByMemberAccess("*.add*"));
	Criteria = Criteria.GetMembersOfTarget();
}

result = Find_Replace() + 
	Find_Parameters() + // Prepared statements
	Find_General_Sanitize() +
	methods.FindByMemberAccess("DriverManager.getConnection") +
	All.FindByType("Connection") +
	Find_Replace_Param() +
	Find_Ibatis_Sanitize() +
	hibernateParameters +
	criteriaAdd
	;// +
	//methods.FindByMemberAccess("StringEscapeUtils.escapeSql");