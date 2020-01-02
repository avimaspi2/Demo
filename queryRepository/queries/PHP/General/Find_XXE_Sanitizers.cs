CxList methods = Find_Methods();
CxList idents = All.FindByType(typeof(UnknownReference));
CxList bools = All.FindByType(typeof(BooleanLiteral));

CxList lxmlNonet = idents.FindByShortName("LIBXML_NONET");
CxList nonetMethod = lxmlNonet.GetAncOfType(typeof(MethodInvokeExpr))+
	lxmlNonet.GetAncOfType(typeof(ObjectCreateExpr));

CxList lxmlDtdAttr = idents.FindByShortName("LIBXML_DTDATTR");
CxList lxmlDtdLoad = idents.FindByShortName("LIBXML_DTDLOAD");
CxList dtdAttrMethod = lxmlDtdAttr.GetAncOfType(typeof(Param));
CxList dtdLoadMethod = lxmlDtdLoad.GetByAncs(dtdAttrMethod);
CxList dtdSanit = dtdLoadMethod.GetAncOfType(typeof(MethodInvokeExpr))+
	dtdLoadMethod.GetAncOfType(typeof(ObjectCreateExpr));

CxList sanit = methods.FindByShortName("libxml_disable_entity_loader");
CxList sanitParams = bools.GetParameters(sanit).FindByShortName("false");
sanit -= sanitParams.GetAncOfType(typeof(MethodInvokeExpr));
CxList sanitized = All.NewCxList();
foreach(CxList c in sanit){
	try{
		int fileId = (c.data.GetByIndex(0) as CSharpGraph).LinePragma.GetFileId();
		sanitized.Add(methods.FindByFileId(fileId));
	}catch(Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}

result = nonetMethod + dtdSanit + sanitized;

result.Add(Find_HTML_Encode());

// Some methods considered sanitizers in php
result.Add(methods.FindByShortNames(new List<string> {
		"urlencode", "rawurlencode"}, false));