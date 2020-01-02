// *URLConnection support
CxList methods = Find_Methods();
CxList urlConnections = All.NewCxList();
CxList urlConnTypes = All.FindByType("*URLConnection");
List<string> urlConnMethods = new List<string>{ "getOutputStream", "write", "getResponseCode" };

urlConnections.Add(methods.FindByShortNames(urlConnMethods));	
urlConnections.Add(All.FindByMemberAccess("*URLConnection", "responseCode"));
urlConnections.Add(All.FindByMemberAccess("*URLConnection", "outputStream"));

result.Add(urlConnections.DataInfluencedBy(urlConnTypes));


// Apache DefaultHttpClient
CxList httpClient = methods.FindByMemberAccess("DefaultHttpClient.execute");
CxList refs = All.FindAllReferences(All.GetParameters(httpClient, 0));
result.Add(httpClient);
result.Add(refs);

// Android WebView.loadUrl
result.Add(methods.FindByMemberAccess("WebView.loadUrl"));

// Retrofit
result.Add(methods.DataInfluencedBy(methods.FindByMemberAccess("Retrofit", "Builder")));

// OKHttp
CxList okHttpClient = methods.FindByMemberAccess("OkHttpClient.newCall");
result.Add(okHttpClient);

// Volley support
CxList volleyAdd = methods.FindByShortName("add");
List<string> volleyMethodNames = new List<string> { "StringRequest", "JsonObjectRequest", 
		"JsonRequest", "JsonArrayRequest" };
CxList stringRequest = All.FindByTypes(volleyMethodNames.ToArray());
result.Add(volleyAdd.DataInfluencedBy(stringRequest));