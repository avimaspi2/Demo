CxList methods = Find_Methods();
CxList hibernateTemplate = methods.FindByShortName("getHibernateTemplate");


List <string> names = new List<string>   {
		"find",
		"findByCriteria",
		"findByExample",
		"findByNamedParam",
		"findByNamedQuery",
		"findByNamedQueryAndNamedParam",
		"findByNamedQueryAndValueBean",
		"findByValueBean",
		"findByNamedQueryAndNamedParam",
		"load",
		"loadAll",
		"iterate"};
		
CxList dbCommands = methods.FindByShortNames(names) +
	All.FindByParameters(All.GetParameters(methods.FindByShortName("get"), 1)) +
	All.GetParameters(methods.FindByShortName("bulkUpdate"));

CxList hibernate = dbCommands.DataInfluencedBy(hibernateTemplate);

foreach(string name in names)
{
	hibernate.Add(methods.FindByMemberAccess("HibernateTemplate." + name));
}	

hibernate.Add(
	All.FindByParameters(All.GetParameters(methods.FindByMemberAccess("HibernateTemplate.get"), 1)) +
	All.GetParameters(methods.FindByMemberAccess("HibernateTemplate.bulkUpdate")));

hibernate = methods * hibernate;

//Hibernate SessionFactory
CxList sessionFactoryList = All.FindByMemberAccess("SessionFactory.getCurrentSession");
sessionFactoryList.Add(All.FindByMemberAccess("SessionFactory.openSession"));
sessionFactoryList.Add(All.FindByMemberAccess("StatelessSession.openStatelessSession"));

//Criteria
CxList hibernateMethods = methods.FindByMemberAccess("Session.createCriteria");
hibernateMethods.Add(methods.FindByMemberAccess("Criteria.createAlias"));
hibernateMethods.Add(methods.FindByMemberAccess("Criteria.add"));
hibernateMethods.Add(methods.FindByMemberAccess("Criteria.addOrder"));

//Hibernate Query
hibernateMethods.Add(methods.FindByMemberAccess("Session.createQuery"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.createSQLQuery"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.getNamedQuery"));

// Hibernate session
hibernateMethods.Add(methods.FindByMemberAccess("Session.load"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.get"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.iterate"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.find"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.persist"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.delete"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.save"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.update"));
hibernateMethods.Add(methods.FindByMemberAccess("Session.saveOrUpdate"));

sessionFactoryList = hibernateMethods.InfluencedBy(sessionFactoryList);

result = sessionFactoryList.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result.Add(methods.FindByMemberAccess("session.load"));
result.Add(methods.FindByMemberAccess("session.find"));