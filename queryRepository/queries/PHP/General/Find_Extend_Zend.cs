//every Zend application contains a Bootstrap class which extends Zend_Application_Bootstrap
CxList trc = All.FindByType(typeof(TypeRefCollection));
CxList extend = All.FindByType(typeof(TypeRef)).GetByAncs(trc);
result = extend.FindByShortName("*Zend*");