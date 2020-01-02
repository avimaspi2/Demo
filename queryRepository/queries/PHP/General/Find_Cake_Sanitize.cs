if(Find_Ctp_Files().Count > 0)
{
	CxList controller = All.FindByType(typeof(ClassDecl)).FindByShortName("*sController");
	//$value=Sanitize::clean($input)
	CxList sanitizer = (All.FindByShortName("Sanitize").GetByAncs(controller)).GetMembersOfTarget();
	
	List < string > sanitizerStrings = new List<string>(new string[] {"clean", "paranoid", "escape", "html"});

	sanitizer = sanitizer.FindByShortNames(sanitizerStrings);

	result = sanitizer;

	//all DB-out methods are sanitizers except query.
	result.Add(Find_Cake_DB_Out() - All.FindByShortName("query"));
	//all DB-In methods are sanitizers except updateAll 
	CxList saveExpr =  Find_Cake_DB_In() - Find_Cake_DB_In_Query() - All.FindByShortName("updateAll");
	result.Add(saveExpr);
}