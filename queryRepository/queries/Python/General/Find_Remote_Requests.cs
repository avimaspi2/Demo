/*
* Finds out all the common external request mechanisms. This will likely include:
* - direct requests (HTTPClient, Web Services request,..);
* - passive requests (such as embedded references, XXE, XML/XSLT resources, documents with external) ; 
* - Also any dynamic hostname / IP address used for connecting to another service: e.g. DB, LDAP, SMTP, raw sockets, etc.
* - Other sources. 
 Add more as needed.
*/
CxList remote_requests = All.NewCxList();

//HTTP Requests
CxList http_requests = Find_Methods_By_Import("requests", new string[]{"get"});
http_requests.Add(Find_Methods_By_Import("urllib2", new string[]{"urlopen", "urlretrieve"}));
http_requests.Add(Find_Methods_By_Import("urllib", new string[]{"request.urlopen", "request.urlretrieve", "parse.urlparse"}));
http_requests.Add(Find_Methods_By_Import("httplib", new string[]{"HTTPConnection", "HTTPSConnection"}));
http_requests.Add(Find_Methods_By_Import("http", new string[]{"client.HTTPConnection", "client.HTTPSConnection"}));
http_requests.Add(Find_Methods_By_Import("httplib2", new string[]{"*request"}));

//cURL Requests
CxList curl_requests = Find_Methods_By_Import("pycurl", new string[]{"*.setopt"});

//JSON-RPC Requests
CxList jsonrpc_requests = Find_Methods_By_Import("jsonrpc", new string[]{"ServiceProxy"});
jsonrpc_requests.Add(Find_Methods_By_Import("pyjsonrpc", new string[]{"HttpClient"}));

//JSON-WSP Requests
CxList jsonwsp_requests = Find_Methods_By_Import("ladon.clients.jsonwsp", new string[]{"JSONWSPClient"});

//SOAP Requests
CxList soap_requests = Find_Methods_By_Import("zeep", new string[]{"Client"});
soap_requests.Add(Find_Methods_By_Import("osa", new string[]{"Client"}));
soap_requests.Add(Find_Methods_By_Import("pysimplesoap.client", new string[]{"SoapClient"}));
soap_requests.Add(Find_Methods_By_Import("suds.client", new string[]{"Client"}));

//XML-RPC requests
CxList xmlrpc_requests = Find_Methods_By_Import("xmlrpclib", new string[]{"ServerProxy"});

//DB, LDAP, SMTP, Socket
CxList db_requests = Find_DB_Connections();
CxList ldap_requests = Find_LDAP_Outputs();
CxList smtp_requests = Find_Methods_By_Import("smtplib", new string[]{"SMTP", "SMTP_SSL", "LMTP"});
CxList socket_requests = Find_Methods_By_Import("socket", new string[]{"*.bind", "*.create_connection", "*.connect", "*.connect_ex"});

//XXE (Same as Medium/Improper_Restriction_of_XXE_Ref)
CxList xxe = Find_XXE_Sax();
xxe.Add(Find_XXE_SaxSanitized_XMLReaders());
xxe.Add(Find_XXE_Expat());
xxe.Add(Find_XXE_lxml());
CxList xxe_requests = xxe.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

remote_requests.Add(http_requests);
remote_requests.Add(curl_requests);
remote_requests.Add(jsonrpc_requests);
remote_requests.Add(jsonwsp_requests);
remote_requests.Add(soap_requests);
remote_requests.Add(xmlrpc_requests);
remote_requests.Add(db_requests);
remote_requests.Add(ldap_requests);
remote_requests.Add(smtp_requests);
remote_requests.Add(socket_requests);
remote_requests.Add(xxe_requests);

result = remote_requests;