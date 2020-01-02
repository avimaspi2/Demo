///
CxList Referer = All.FindByName("\"Referer\"");
CxList header = All.FindByMemberAccess("request.getHeader");
header = header.DataInfluencedBy(Referer);

CxList ifStmt = Find_Ifs();

ifStmt = All.DataInfluencedBy(header).GetFathers() * ifStmt;

CxList setSessionAttribute = All.FindByMemberAccess("getSession.setAttribute");
setSessionAttribute = setSessionAttribute.GetByAncs(ifStmt);

result = setSessionAttribute;