CxList methods = Find_Methods();

CxList permits = Find_Strong_Parameters();

// if Rails 4 it has Strong Parameters sanitizer
// so it does not require the deprecated attr_accessible
if(permits.Count == 0){
	CxList attrInClass = methods.FindByShortName("attr_accessible");
	CxList model = Find_Models();

	result = model - model.GetClass(attrInClass);
}