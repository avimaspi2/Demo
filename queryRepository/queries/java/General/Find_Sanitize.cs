// SQL - sanitize

CxList methods = Find_Methods();

CxList hibernate = Find_Hibernate_DB();

CxList hibernateParam_All = hibernate.FindByShortNames(new List<string> {
		"get",
		"load",
		"findByCriteria",
		"findByExample",
		"findByNamedQuery*"});


// Sanitizer for JSF: All convertors (including custom-made) and validators
CxList JSFSanitizers = All.FindByShortNames(new List<string> {
		"convertDateTimeJSF",
		"convertNumberJSF",
		"converterGenericJSF",
		"validateDoubleRangeJSF",
		"validateLongRangeJSF",
		"validateRegexJSF",
		"validatorGenericJSF"});

CxList bulkUpdate = All.FindByParameters(hibernate).FindByShortName("bulkUpdate");
CxList hibernateParameters = All.GetParameters(bulkUpdate, 1);

CxList hibernateParam_NotFirst = hibernate.FindByShortName("find*");
hibernateParam_NotFirst.Add(hibernate.FindByShortName("iterate"));

hibernateParameters.Add(All.GetParameters(hibernateParam_NotFirst));
hibernateParameters -= hibernateParameters.GetParameters(hibernateParam_NotFirst, 0);
hibernateParameters.Add(All.GetParameters(hibernateParam_All));

CxList criteriaAdd = All.NewCxList();
CxList Criteria = All.FindByShortNames(new List<string> {
		"*Criteria",
		"Subcriteria",
		"CriteriaImpl",
		"Projections"});
	
Criteria.Add(All.FindByTypes(new string[] {
	"*Criteria",
	"Subcriteria",
	"CriteriaImpl",
	"Projections",
	"CriteriaSpecification"}));

int count = 25;
while (Criteria.Count > 0 && count-- > 0)
{
	criteriaAdd.Add(Criteria.FindByMemberAccess("*.add*"));
	Criteria = Criteria.GetMembersOfTarget();
}

result = Find_Replace();
 
result.Add(Find_Parameters()); // Prepared statements
result.Add(Find_General_Sanitize());
result.Add(methods.FindByMemberAccess("DriverManager.getConnection"));
result.Add(methods.FindByShortName("getUserPrincipal"));
result.Add(All.FindByType("Connection"));
result.Add(Find_Replace_Param());
result.Add(Find_Ibatis_Sanitize());
result.Add(Find_MyBatis_Sanitize());
result.Add(hibernateParameters);
result.Add(criteriaAdd);
result.Add(JSFSanitizers);
result.Add(Find_Hibernate_Sanitize());
result.Add(Find_Whitelisting());


// If it is an Android project - Add Android DB
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Sanitize());
}