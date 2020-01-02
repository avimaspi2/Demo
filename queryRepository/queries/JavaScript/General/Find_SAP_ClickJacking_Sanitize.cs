// Look for: window["sap-ui-config"] = { frameOptions: 'trusted'/'deny'

CxList declaratorsUnknownRef = Find_Declarators();
declaratorsUnknownRef.Add(Find_UnknownReference());

CxList frameOptions = declaratorsUnknownRef.FindByShortName("frameOptions");
CxList frameOptionsValue = frameOptions.GetAssigner().FindByShortNames(new List<string> {"trusted", "deny"});

result = frameOptionsValue.GetByAncs(frameOptionsValue); 
// Look for: <meta name="sap.whitelistService" content="url/to/whitelist/service" />
CxList WhiteListMeta = All.FindByRegexExt("<\\s*meta[\\w\\W]*name\\s*=\\s*\"sap.whitelistService\" [\\w\\W]*>", "*.*", true, RegexOptions.None);
result.Add(WhiteListMeta);
// Look for: <script data-sap-ui-frameOptions='trusted'/'deny' >
CxList data_sap_ui_frameOptions = All.FindByRegexExt("<\\s*script[\\w\\W]*data-sap-ui-frameOptions\\s*=\\s*[\'\"](deny|trusted)[\'\"]", "*.*", false, RegexOptions.None);
result.Add(data_sap_ui_frameOptions);