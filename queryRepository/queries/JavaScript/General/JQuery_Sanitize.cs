//Methods that are sanitized in JQuery

CxList Methods = Find_JQuery_Methods();

result = Methods.FindByShortNames(new List<string>{
		"hasClass","is","height","width","innerHeight",
		"innerWidth","outerHeight","outerWidth","offset",
		"position","scrollTop","scrollLeft","inArray",
		"isArray","isEmptyObject","isNumeric","isPlainObject",
		"isWindow","isXMLDoc","type","text"});