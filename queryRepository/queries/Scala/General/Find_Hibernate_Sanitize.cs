CxList hibernate = Find_Hibernate_DB();

CxList hibernate_not_Sanizited = hibernate.FindByShortNames(new List<string> {
		"createQuery",
		"createSQLQuery",
		"getNamedQuery",
		"find",
		});

hibernate -= hibernate_not_Sanizited;

hibernate.Add(All.GetParameters(hibernate));

result = hibernate;