CxList methods = Find_Methods();

CxList loadDOM = methods.FindByMemberAccess("XMLReader.read") + 
	methods.FindByMemberAccess("XMLReader.open") + 
	methods.FindByMemberAccess("XMLReader.xml") ;

result = loadDOM;