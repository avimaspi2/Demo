CxList methods = Find_Methods();

result = methods.FindByShortName("setHTML");
result.Add(methods.FindByShortName("setInnerHTML"));
result.Add(methods.FindByMemberAccess("DOM.set*"));
result -= methods.FindByMemberAccess("DOM.setEventListener");