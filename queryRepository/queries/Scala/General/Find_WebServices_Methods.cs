//Find all @GET, @POST, @PUT, @DELETE and @HEAD methods
CxList customAttributes = Find_CustomAttribute();
CxList webServiceMethods = customAttributes.FindByShortNames(new List<String> {
		"GET", "PUT", "POST", "DELETE", "HEAD"});
result = webServiceMethods.GetFathers();