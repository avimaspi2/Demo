CxList methods = Find_Methods();

CxList cookie = methods.FindByMemberAccess("GwtServletBase.getCookie");
cookie.Add(methods.FindByMemberAccess("LocaleUtils.getCookie"));
cookie.Add(methods.FindByMemberAccess("Util.getCookie"));
cookie.Add(methods.FindByMemberAccess("UrlRequestTransport.getCookies"));

result = cookie;