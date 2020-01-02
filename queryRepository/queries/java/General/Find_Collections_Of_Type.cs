//This query searchs for collections of a given type
if(param.Length > 0){

	try{
		
		string[] types;
		
		if(param.Length == 1)
			types = new string[]{param[0] as string};
		else 
			types = param as string[];
		
		CxList collectionsCandidates = Find_Collections().FindByType(typeof(TypeRef));
		CxList collectionsOfType = All.FindByTypes(types).GetFathers().GetAncOfType(typeof(GenericTypeRef));
		CxList collections = (collectionsOfType * collectionsCandidates).GetFathers();

		result.Add(Find_Declarators());
		result.Add(Find_Field_Decl());
		result.Add(Find_ParamDecl());
		result.Add(Find_Constants());
		result.Add(base.Find_PropertyDecl());
		
		result = result.GetByAncs(collections);
		result.Add(Find_UnknownReference().FindAllReferences(result));
					
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);	
	}

}