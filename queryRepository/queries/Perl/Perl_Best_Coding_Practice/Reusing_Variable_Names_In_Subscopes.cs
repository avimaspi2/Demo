/*
see: https://www.securecoding.cert.org/confluence/display/perl/DCL01-PL.+Do+not+reuse+variable+names+in+subscopes

Do not use the same variable name in two scopes where one scope is contained in another.

For example,
* A lexical variable should not share the name of a package variable if the lexical variable is in 
a subscope of the global variable.
* A block should not declare a lexical variable with the same name as a lexical variable declared in
any block that contains it.

Reusing variable names leads to programmer confusion about which variable is being modified. Additionally,
if variable names are reused, generally one or both of the variable names are too generic.

Example:
$errmsg = "Global error";
 
sub report_error {
  my $errmsg = shift(@_);
}
*/

// Find all assign expressions' left side members
CxList leftAssign = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
leftAssign = leftAssign.FindByShortName("$*") + leftAssign.FindByShortName("%*");
leftAssign -= leftAssign.FindByShortName("$_");

// Get the class and method of this assign expression
CxList leftAssignClasses = All.GetClass(leftAssign);
CxList leftAssignMethods = All.GetMethod(leftAssign);

// Pass on all classes with left assign, and look for variable names in subscopes
foreach (CxList cls in leftAssignClasses)
{
	CxList clsLeftAssign = leftAssign.GetByAncs(cls); // expressions' left side members in this class
	CxList clsMethods = leftAssignMethods.GetByAncs(cls); // methods with assign expressions in this class
	CxList artificialMethods = clsMethods.FindByShortName("Method_*"); // Cx-created artificial methods
	CxList realMethods = clsMethods - artificialMethods; // Methods that are inside classes and not directly in a package
	CxList leftAssignInReal = clsLeftAssign.GetByAncs(realMethods).FindByType(typeof(Declarator)); // check only Declarators
	CxList leftAssignArtificial = clsLeftAssign.GetByAncs(artificialMethods); // Get the assigns that appear directly in the package
	// loop on all assign expressions in methods and return all that have also an assignment outside the method,
	// i.e. directly in the class
	
	// Remove assigns on regex substitutions
	CxList regexAssigns = Find_Regex().GetAncOfType(typeof(AssignExpr));
	leftAssignArtificial -= leftAssignArtificial.GetByAncs(regexAssigns);
	
	foreach (CxList singleAssignInReal in leftAssignInReal)
	{
		CxList assignInArtificial = leftAssignArtificial.FindByShortName(singleAssignInReal);
		result.Add(assignInArtificial.ConcatenateAllSources(singleAssignInReal));
	}	
}