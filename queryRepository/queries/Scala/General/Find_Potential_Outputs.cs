CxList methods = Find_Methods();
CxList actionClass = Find_Action_Classes();

/* struts */
/// SaveMessages in Action classes (struts)
CxList saves = methods.FindByShortNames(new List<string> {"saveMessages","saveErrors","addErrors"});
//	methods.FindByShortName("saveMessages") + 
//	methods.FindByShortName("saveErrors") +
//	methods.FindByShortName("addErrors");
saves = All.GetParameters(saves, 1);
saves = saves.GetByAncs(actionClass);

CxList struts2Outputs = All.NewCxList();
CxList struts2Outputs2 = All.GetByAncs(actionClass).FindByType(typeof(MethodDecl)).FindByShortName("get*");
struts2Outputs2 = All.GetByAncs(struts2Outputs2);
struts2Outputs = struts2Outputs2.FindByType(typeof(ReturnStmt));
struts2Outputs = struts2Outputs2.FindByFathers(struts2Outputs);

struts2Outputs.Add(Find_Struts_Messages());

CxList mav = All.FindByMemberAccess("ModelAndView.addObject");
mav = All.GetParameters(mav, 1);

CxList setAttr = 
	All.FindByMemberAccess("HttpServletRequest.setAttribute") +
	All.FindByName("*request.setAttribute") + 
	All.FindByName("*Request.setAttribute");

CxList getSession = 
	All.FindByMemberAccess("HttpServletRequest.getSession") +
	All.FindByName("*request.getSession") +  
	All.FindByName("*Request.getSession");

setAttr.Add(getSession.GetMembersOfTarget().FindByShortName("setAttribute"));
setAttr = All.GetParameters(setAttr, 1);

CxList ModelAndView = Find_Object_Create().FindByShortName("ModelAndView");
CxList mavParams = All.GetParameters(ModelAndView);
CxList put = methods.FindByName("*.put");
CxList model = put.GetTargetOfMembers();
CxList mavModel = mavParams.FindAllReferences(model);
model *= model.DataInfluencingOn(All.DataInfluencingOn(mavModel));
put = model.GetMembersOfTarget();


CxList inActionClass = All.GetByAncs(actionClass);
CxList session = methods.FindByShortName("getSession");
CxList map = inActionClass.FindByType("Map");
map.Add(inActionClass.FindByType(typeof(DictionaryCreateExpr)));
session = map.DataInfluencedBy(session);
CxList sessionAware = inActionClass.InheritsFrom("SessionAware");

CxList sessionTarget = session.GetMembersOfTarget();
CxList sessionPut = sessionTarget.FindByShortName("put");
CxList sessionPutParams = inActionClass.GetByAncs(All.GetParameters(sessionPut, 1));

result = saves;
result.Add(struts2Outputs);
result.Add(mav);
result.Add(setAttr);
result.Add(put); 
result.Add(sessionPutParams);
result -= Find_Interactive_Outputs();