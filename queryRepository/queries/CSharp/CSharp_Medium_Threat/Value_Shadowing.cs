CxList variables = All.FindByType(typeof(IndexerRef));

CxList problematic = variables.FindByTypes(new string[]{"Request","HttpRequest"});

foreach(KeyValuePair<int,IGraph> elem in problematic.data){
	try{
		IndexerRef ir = elem.Value as IndexerRef;
		CSharpGraph el = ir.Indices[0];
		result.Add(el.NodeId, el);
	}catch(Exception exc){
		cxLog.WriteDebugMessage(exc);
	}
}