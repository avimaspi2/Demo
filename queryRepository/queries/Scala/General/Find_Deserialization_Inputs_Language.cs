CxList Ifs = All.FindByType(typeof(IfStmt));
CxList expressions = All.FindByType(typeof(Expression));

CxList xmlBeans = All.FindByAssignmentSide(CxList.AssignmentSide.Left)
	.GetByAncs(expressions.FindByName("bean", false).FindByFathers(Ifs).GetFathers());

CxList MarshalingHandlers = xmlBeans.FindByName("struts.bean.type", false).GetAssigner().FindByShortName("org.apache.struts2.rest.handler.ContentTypeHandler");
CxList IfOfMarshalingHandlers = MarshalingHandlers.GetAncOfType(typeof(IfStmt));
CxList MarshalingXMLHandlers = xmlBeans.FindByName("struts.bean.name", false).GetAssigner().FindByShortName("xml");
CxList IfOfMarshalingXMLHandlers = MarshalingXMLHandlers.GetAncOfType(typeof(IfStmt));
CxList Class = xmlBeans.FindByName("struts.bean.class", false).GetAssigner();
CxList ClassInXML = Class.GetByAncs(IfOfMarshalingHandlers * IfOfMarshalingXMLHandlers);

CxList ClassDecls = Find_Class_Decl();
CxList ClassesExtendingContentTypeHandler = All.NewCxList();
foreach(CxList cls in ClassInXML){
	ClassesExtendingContentTypeHandler.Add(ClassDecls.FindByName(cls.GetName()));
}
CxList toObjectMethod = Find_MethodDeclaration().GetByAncs(ClassesExtendingContentTypeHandler).FindByShortName("toObject");

result = All.GetParameters(toObjectMethod).FindByType("Reader");