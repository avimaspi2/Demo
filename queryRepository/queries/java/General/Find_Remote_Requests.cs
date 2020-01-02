/*
* Finds out all the common external request mechanisms. This will likely include:
* - direct requests (HTTPClient, Web Services request,..);
* - passive requests (such as embedded references, XXE, XML/XSLT resources, documents with external) ; 
* - Also any dynamic hostname / IP address used for connecting to another service: e.g. DB, LDAP, SMTP, raw sockets, etc.
* - Other sources. 
*/

CxList methods = Find_Methods();

CxList writters = methods.FindByMemberAccess("DataOutputStream.writeBytes");
writters.Add(methods.FindByMemberAccess("DataOutputStream.write"));
writters.Add(methods.FindByMemberAccess("BufferedWriter.write"));
writters.Add(methods.FindByMemberAccess("PrintWriter.print"));
writters.Add(methods.FindByMemberAccess("OutputStreamWriter.write"));
writters.Add(methods.FindByMemberAccess("OutputStream.write"));

CxList remote_requests = All.NewCxList();

//HTTPClients requests
remote_requests.Add(methods.FindByMemberAccess("HttpClient.execute*"));
remote_requests.Add(methods.FindByMemberAccess("URL.openConnection"));
remote_requests.Add(methods.FindByMemberAccess("Connector.open"));
remote_requests.Add(methods.FindByMemberAccess("Request.execute")); //org.apache.http.client.fluent.Request;
remote_requests.Add(Find_XSRF_Requests());

//WebServices requests
remote_requests.Add(methods.FindByMemberAccess("SoapConnection.call"));

//To do a POST with HttpURLConnection and other,
// you need to write the parameters to the connection after you have opened the connection.
CxList output_streams = methods.FindByMemberAccess("Socket.getOutputStream");
output_streams.Add(methods.FindByMemberAccess("URLConnection.getOutputStream"));
output_streams.Add(methods.FindByMemberAccess("HttpsURLConnection.getOutputStream"));
output_streams = output_streams.DataInfluencingOn(writters).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
remote_requests.Add(output_streams);

//cURL
remote_requests.Add(methods.FindByMemberAccess("URL.openStream"));
remote_requests.Add(methods.FindByMemberAccess("Runtime.exec"));

//dynamic hostname/IP address for DB, LDAP, SMTP, raw sockect ...
remote_requests.Add(All.GetParameters(methods.FindByMemberAccess("DriverManager.getConnection")));
remote_requests.Add(All.GetParameters(methods.FindByMemberAccess("DataSource.getConnection")));
remote_requests.Add(All.GetParameters(methods.FindByMemberAccess("Transport.send")));
remote_requests.Add(Find_LDAP_Outputs());

// XXE
remote_requests.Add(Find_XXE_JAXP_DOM());
remote_requests.Add(Find_XXE_JAXP_SAX());
remote_requests.Add(Find_XXE_XMLReader());
remote_requests.Add(Find_XXE_JAXB());
remote_requests.Add(Find_XXE_StAX());
remote_requests.Add(Find_XXE_DOM4J());
remote_requests.Add(Find_XXE_XOM());

//ftp
remote_requests.Add(methods.FindByMemberAccess("FTPClient.connect"));
remote_requests.Add(methods.FindByMemberAccess("FTPClient.retrieveFile"));
remote_requests.Add(methods.FindByMemberAccess("FTPClient.retrieveFileStream"));

result = remote_requests;