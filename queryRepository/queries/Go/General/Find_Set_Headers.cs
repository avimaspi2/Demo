//The list where the Header is defined
CxList responseWriterHeaders = Find_Methods().FindByMemberAccess("http.ResponseWriter", "Header", false); 
CxList responseWriterHeaderOccurrences = responseWriterHeaders;
responseWriterHeaderOccurrences.Add(Find_UnknownReferences().DataInfluencedBy(responseWriterHeaders));

//The list were are executed Sets in the Headers
result = responseWriterHeaderOccurrences.GetMembersOfTarget().FindByShortName("Set");