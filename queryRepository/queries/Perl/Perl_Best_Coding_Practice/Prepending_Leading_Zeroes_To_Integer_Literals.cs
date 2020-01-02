/*
Taken from: https://www.securecoding.cert.org/confluence/display/perl/INT00-PL.+Do+not+prepend+leading+zeroes+to+integer+literals
When representing numeric literal values, Perl has a simple rule:
integers that are prefixed with one or more leading zeroes are interpreted as octal,
and integers with no leading zero are interpreted as decimal.

While simple, this rule is not known among many developers and is not obvious to those unware of it.
Consequently, do not prefix an integer with leading zeros.
If it is to be interpreted as octal, use the oct() function, which clearly indicates the number to be treated as octal.

my $perm1 = 0644;      # noncompliant, octal
my $perm2 = "0644";    # noncompliant, decimal
my $perm3 = oct(644);  # compliant, octal
my $perm4 = 644;       # compliant, decimal
*/

CxList decl = All.FindByType(typeof(VariableDeclStmt));
decl = All.GetByAncs(decl);

CxList result1 = 
	decl.FindByShortName("01*") + 
	decl.FindByShortName("02*") + 
	decl.FindByShortName("03*") + 
	decl.FindByShortName("04*") + 
	decl.FindByShortName("05*") + 
	decl.FindByShortName("06*") + 
	decl.FindByShortName("07*") + 
	decl.FindByShortName("08*") + 
	decl.FindByShortName("09*");
result1 -= result1.FindByRegex("'0"); // remove strings
	
result = result1 + decl.FindByRegex(@"=\s*0+[1-9]");

// Remove parameters
result -= result.GetByAncs(result.GetAncOfType(typeof(Param)));