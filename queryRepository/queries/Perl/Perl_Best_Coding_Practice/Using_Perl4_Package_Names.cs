/*
see: https://www.securecoding.cert.org/confluence/display/perl/DCL05-PL.+Prohibit+Perl4+package+names
This noncompliant code example uses the Perl 4 ' syntax to import an external package.
This code does successfully require the package, but because Perl 5 is over 15 years ago,
the Perl 4 syntax has largely been forgotten. Consequently, the code can be seen as confusing or arcane.

require DBI'SQL'Nano;
*/

result = All.FindByRegex(@"require\s+\w+'");