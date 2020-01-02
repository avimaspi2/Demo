CxList inputs = Find_Interactive_Inputs();
CxList declarators = Find_Declarators();
CxList paramss = base.Find_ParamDecl();

CxList declParams = declarators;
declParams.Add(paramss);

CxList stringDeclaratorsAndParams = declParams.FindByType("String");
CxList unkRefs = Find_UnknownReference();
CxList unkRefsAndDeclAndParams = unkRefs;
unkRefsAndDeclAndParams.Add(declarators);
unkRefsAndDeclAndParams.Add(paramss);

CxList stringDeclaratorsReferences = unkRefsAndDeclAndParams.FindAllReferences(stringDeclaratorsAndParams);
inputs = inputs.InfluencingOn(stringDeclaratorsReferences);

CxList requests = Find_Remote_Requests();
CxList sanitizers = Find_Remote_Requests_Sanitize();
	
result = requests.InfluencedByAndNotSanitized(inputs, sanitizers);