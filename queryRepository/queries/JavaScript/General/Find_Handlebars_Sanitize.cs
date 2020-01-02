if(cxScan.IsFrameworkActive("Handlebars"))
{
	CxList invokes = Find_Methods();
	CxList helpers = All.FindByMemberAccess("Handlebars.registerHelper") * invokes;
	CxList lambdas = Find_LambdaExpr();
	CxList paramts = All.GetParameters(helpers, 1);
	CxList methods = lambdas * paramts;

	// Function pointer case:
	// var func = function(person) {return options.fn(this);}
	// Handlebars.registerHelper('fullName', func);
	CxList unknown = Find_UnknownReference();
	CxList pointers = unknown * paramts;

	CxList defs = All.FindDefinition(pointers);
	CxList refs = All.FindAllReferences(defs);

	CxList assignees = refs.GetAssigner(lambdas);
	methods.Add(assignees);

	// Find options.fn() invoke that are inside a helper function and add them as sanitizers because
	// contents of a block helper are escaped when options.fn(context) is called
	CxList fn = invokes.FindByShortName("fn").GetMembersWithTargets();
	result = fn.GetByAncs(methods);

	result.Add(All.FindByMemberAccess("Utils.escapeExpression"));
	result.Add(All.FindByMemberAccess("Handlebars.escapeExpression"));

	// Treat the escape case of handlebars outputs in the views
	// this is, the {{value}} is an output sanitizer, while {{{value}}} is not
	// Internally, these are seen as CxOutput(escape(value)) and CxOutput(value).
	CxList hbs_outputs = Find_Handlebars_Outputs();
	CxList hbs_outputs_sanitizers = invokes.FindByShortName("escape").GetFathers().GetAncOfType(typeof(MethodInvokeExpr)) * hbs_outputs;
	result.Add(hbs_outputs_sanitizers);
}