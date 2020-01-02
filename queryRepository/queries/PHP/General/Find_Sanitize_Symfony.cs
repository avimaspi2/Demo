CxList methods = Find_Methods();
//When using Doctrine Symfony ORM, setParameter is a sanitizer by itself
CxList hasDoctrine = methods.FindByShortName("*doctrine*", false);

if(hasDoctrine.Count > 0) {
	CxList createQuery = methods.FindByShortNames(new List<String>{"createQuery", "createNamedQuery", "createQueryBuilder"}, true);
	CxList setParameter = methods.FindByShortName("setParameter", true);
	CxList setParameters = methods.FindByShortName("setParameters", true);
	
	CxList sanitizedMethods = (setParameter + setParameters).DataInfluencedBy(createQuery);
	sanitizedMethods = sanitizedMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	result = sanitizedMethods;
}