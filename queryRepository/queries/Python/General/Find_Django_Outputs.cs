/*** 
This query finds outputs from Django

Documentation for Templates: https://docs.djangoproject.com/en/dev/ref/templates/api/
							 https://docs.djangoproject.com/en/dev/topics/http/shortcuts/
Documentation for HttpResponse: https://docs.djangoproject.com/en/1.7/ref/request-response/#django.http.HttpResponse
***/
if (Find_Django().Count != 0)
{
	CxList imports = Find_Imports();
	CxList methods = Find_Methods();
	//Django Templates
	String[] djangoSMethods = new string[] {"render_to_response"};
	String[] djangoTemplateMethods = new string[] {"Template","loader","render_to_string"};

	CxList djangoTemplatesList = Find_Methods_By_Import("django.template*", djangoTemplateMethods, imports);

	CxList djangoTemplates = djangoTemplatesList.FindByShortName("Template");
	CxList allInfluencedByLoader = All.InfluencedBy(djangoTemplatesList.FindByShortName("loader"));

	CxList djangoRenderMethodsList = Find_Methods_By_Import("django.shortcuts", djangoSMethods, imports);
	djangoRenderMethodsList.Add(djangoTemplatesList.FindByShortName("render_to_string"));
	djangoRenderMethodsList.Add(allInfluencedByLoader.FindByShortName("render"));
	djangoRenderMethodsList.Add(allInfluencedByLoader.GetMembersOfTarget().FindByShortName("render"));
	djangoRenderMethodsList.Add(All.InfluencedBy(djangoTemplates).GetMembersOfTarget().FindByShortName("render"));
		
	CxList unknownReference = Find_UnknownReference();
	//This will be used only to find the interesting methods
	CxList cxDjangoTemplates = unknownReference.FindByShortName("CxDjangoTemplate");	
		
	//Transformed render and render_to_response methods
	List<string> methodsList = new List<string>(){"render_to_response","render"};
	CxList djangoRenderMethodsTransformed = cxDjangoTemplates.GetMembersOfTarget() * methods.FindByShortNames(methodsList);
	
	//Django HttpResponse
	String[] djangoHttpResp = new string[] {"HttpResponse", "HttpResponseRedirect", 
	"HttpResponsePermanentRedirect", "HttpResponseNotModified", "HttpResponseBadRequest",
	"HttpResponseNotFound", "HttpResponseForbidden", "HttpResponseNotAllowed",
	"HttpResponseGone", "HttpResponseServerError", "JsonResponse"};

	CxList djangoHttpRespObj 
		= Find_Methods_By_Import("django.http", djangoHttpResp, imports);

	CxList djangoHttpRespList = All.InfluencedBy(djangoHttpRespObj).GetMembersOfTarget();

	CxList djangoHttpRespMethods = djangoHttpRespList.FindByName("write");
	djangoHttpRespMethods.Add(djangoHttpRespList.FindByName("set_signed_cookie"));
	djangoHttpRespMethods.Add(djangoHttpRespList.FindByName("set_cookie"));

	result = djangoRenderMethodsList;
	result.Add(djangoHttpRespObj);
	result.Add(djangoHttpRespMethods);
	result.Add(djangoRenderMethodsTransformed);
}
else
{
	result = All.NewCxList();
}