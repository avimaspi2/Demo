// this query checks if a self signed certificate coming from the server is validated against the client one

CxList X509Certificate = All.FindByType("X509Certificate").GetAncOfType(typeof(TypeRef));  
CxList ArrayCreate = X509Certificate.GetAncOfType(typeof(ArrayCreateExpr));
CSharpGraph ArrayCreateFather = ArrayCreate.GetFathers().TryGetCSharpGraph<CSharpGraph>();

if (ArrayCreateFather != null && ArrayCreateFather.GraphType == GraphTypes.ReturnStmt)
{
	CxList ReturnUnValidateCertificate = ArrayCreate.GetAncOfType(typeof(ReturnStmt));
	CxList getAcceptedIssuersMethods = ReturnUnValidateCertificate.GetAncOfType(typeof(MethodDecl)).FindByShortName("getAcceptedIssuers");
	foreach (CxList getAcceptedIssuersMethod in getAcceptedIssuersMethods)
	{
		CxList ParentClass = getAcceptedIssuersMethod.GetAncOfType(typeof(ClassDecl));
		if (ParentClass.InheritsFrom("X509TrustManager").Count > 0)
		{
			result = getAcceptedIssuersMethod;
		}
	}		 
}