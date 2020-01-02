CxList methods = Find_Methods();
CxList hibernateTemplate = methods.FindByShortName("getHibernateTemplate");
CxList dbCommands =
	methods.FindByShortName("find") +
	methods.FindByShortName("findByCriteria") +
	methods.FindByShortName("findByExample") +
	methods.FindByShortName("findByNamedParam") +
	methods.FindByShortName("findByNamedQuery") +
	methods.FindByShortName("findByNamedQueryAndNamedParam") +
	methods.FindByShortName("findByNamedQueryAndValueBean") +
	methods.FindByShortName("findByValueBean") +
	methods.FindByShortName("findByNamedQueryAndNamedParam") +
	methods.FindByShortName("load") +
	methods.FindByShortName("loadAll") +
	All.FindByParameters(All.GetParameters(methods.FindByShortName("get"), 1)) +
	All.GetParameters(methods.FindByShortName("bulkUpdate")) +
	methods.FindByShortName("iterate");
CxList hibernate = dbCommands.DataInfluencedBy(hibernateTemplate);
hibernate.Add(
	methods.FindByMemberAccess("HibernateTemplate.find") +
	methods.FindByMemberAccess("HibernateTemplate.findByCriteria") +
	methods.FindByMemberAccess("HibernateTemplate.findByExample") +
	methods.FindByMemberAccess("HibernateTemplate.findByNamedParam") +
	methods.FindByMemberAccess("HibernateTemplate.findByNamedQuery") +
	methods.FindByMemberAccess("HibernateTemplate.findByNamedQueryAndNamedParam") +
	methods.FindByMemberAccess("HibernateTemplate.findByNamedQueryAndValueBean") +
	methods.FindByMemberAccess("HibernateTemplate.findByValueBean") +
	methods.FindByMemberAccess("HibernateTemplate.findByNamedQueryAndNamedParam") +
	methods.FindByMemberAccess("HibernateTemplate.load") +
	methods.FindByMemberAccess("HibernateTemplate.loadAll") +
	All.FindByParameters(All.GetParameters(methods.FindByMemberAccess("HibernateTemplate.get"), 1)) +
	All.GetParameters(methods.FindByMemberAccess("HibernateTemplate.bulkUpdate")) +
	methods.FindByMemberAccess("HibernateTemplate.iterate")
	);

hibernate = methods * hibernate;

// Hibernate session
hibernate.Add(All.FindByMemberAccess("session.load") + All.FindByMemberAccess("session.get") +
	All.FindByMemberAccess("session.iterate") + All.FindByMemberAccess("session.find"));

result = hibernate;