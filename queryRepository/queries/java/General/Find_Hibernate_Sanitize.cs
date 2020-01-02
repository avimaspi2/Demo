CxList hibernate = Find_Hibernate_DB();
CxList methods = Find_Methods();
CxList hibernate_not_Sanizited = hibernate.FindByShortNames(new List<string> {
		"createQuery",
		"createSQLQuery",
		"getNamedQuery",
		"find",
		});

hibernate -= hibernate_not_Sanizited;
hibernate.Add(All.GetParameters(hibernate));

// Sanitizers methods list
List < string > setMethods = new List<string>{
		"setBoolean",
		"setByte",
		"setDouble",
		"setFloat",
		"setLong",
		"setInteger",
		"setParameter",
		"setParameterList",
		"setString",
		"setTimestamp",
		"setProperties",
		"setEntity"
		};

// Add methods for cases such as:
//     createQuery(...)
//  	.setParameter();
//
CxList sanitize_methods = methods.FindByShortNames(setMethods);
CxList hibernate_sanitize = hibernate_not_Sanizited.DataInfluencingOn(sanitize_methods);
hibernate_sanitize = hibernate_sanitize.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
hibernate.Add(hibernate_sanitize);

// Add methods from Query.setXXX()
CxList queryMethods = sanitize_methods.FindByMemberAccess("query.set*");
hibernate.Add(queryMethods.FindByShortNames(setMethods));

// Add method from MethodInvoke composition such as
// Query.setChangeable()
//		.setxxx()
CxList composition = hibernate_sanitize.GetTargetOfMembers();

CxList composition_target = All.NewCxList();
composition_target.Add(composition);

for(int i = 0; i < 5; i++){
	composition.Add(composition_target);
	composition_target = composition_target.GetTargetOfMembers();
}
hibernate.Add(composition.FindByType("*query"));

result = hibernate;