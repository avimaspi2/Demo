/*In this query we search for controllers that works with files*/
CxList mvcControllers = Find_ASP_MVC_Controllers();

/*Search for controllers ActionResult and controllers FileResult */
CxList FileResultMethods = mvcControllers.FindByMethodReturnType("FileResult");
FileResultMethods.Add( mvcControllers.FindByMethodReturnType("ActionResult"));

/*Search for object FilePathResult and method File in our controllers list*/
CxList files = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("FilePathResult").GetByAncs(FileResultMethods);
files.Add(Find_Methods().FindByShortName("File").GetByAncs(FileResultMethods));

result.Add(All.GetParameters(files, 0));