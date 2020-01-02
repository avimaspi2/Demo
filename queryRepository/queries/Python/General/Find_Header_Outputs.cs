// Django
string[] djangoHttpResp = new string[] {"HttpResponse*", "JsonResponse"};

CxList djangoHttpRespObj = Find_Methods_By_Import("django.http", djangoHttpResp);

result.Add(djangoHttpRespObj.GetAncOfType(typeof(IndexerRef)));

result.Add(Find_HTTPServer_Outputs().FindByShortName("send_header"));