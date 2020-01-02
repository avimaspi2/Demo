CxList arrays = All.FindByType(typeof(IndexerRef));
CxList unknown = All.FindByType(typeof(UnknownReference));
arrays = 
	arrays.FindByShortName("$*") + 
	arrays.FindByShortName("@*") + 
	arrays.FindByShortName("%*");
arrays -= arrays.FindByShortName("$_");
arrays -= arrays.FindByShortName("@_");
arrays -= arrays.FindByShortName("%_");
CxList def = All.FindDefinition(arrays);

CxList outputs = arrays - arrays.FindAllReferences(def);

result = unknown.FindAllReferences(outputs.FindByAssignmentSide(CxList.AssignmentSide.Left));