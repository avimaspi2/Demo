///
CxList Referer = All.FindByName("\"Referer\"");
CxList header = All.FindByMemberAccess("request.getHeader");
header = header.DataInfluencedBy(Referer);

CxList ifStmt = All.FindByType(typeof(IfStmt));

ifStmt = All.DataInfluencedBy(header).GetFathers() * ifStmt;

CxList setSessionAttribute = All.FindByMemberAccess("getSession.setAttribute");
setSessionAttribute = setSessionAttribute.GetByAncs(ifStmt);

result = setSessionAttribute;