List<string> resultTypes = new List<string> {
		"ActionResult",
		"ViewResult",
		"PartialViewResult",
		"ContentResult",
		"FileResult",
		"EmptyResult",
		"HttpStatusCodeResult",
		"JavaScriptResult",
		"JsonResult",
		"RedirectResult",
		"IHttpActionResult"};

CxList actionResults = All.FindByShortNames(resultTypes, false);
actionResults.Add(All.InheritsFrom(actionResults));

result = All.FindByType(typeof(MethodDecl)).GetByAncs(All.GetClass(actionResults.GetFathers()));