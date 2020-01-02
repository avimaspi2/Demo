/*
see: https://www.securecoding.cert.org/confluence/display/perl/DCL00-PL.+Do+not+use+subroutine+prototypes

Perl provides a simple mechanism for specifying subroutine argument types called prototypes.
Prototypes do not affect functions defined using the & character. Furthermore, the perlfunc manpage states:
   Method calls are not influenced by prototypes either, because the function to be called is indeterminate at
   compile time, since the exact code called depends on inheritance.

Prototypes do not cause Perl to emit any warnings if a subroutine's invocation uses methods that don't
match its prototype, not even if the -w switch is used. They also can change function behavior,
and consequently should not be used.

This is what we are looking for: 
sub function ($@){
...
}
*/

CxList paramDeclCollection = All.FindByType(typeof(ParamDeclCollection)).FindByFathers(All.FindByType(typeof(MethodDecl)));
CxList paramDecl = All.FindByFathers(paramDeclCollection);

result = paramDecl.FindByShortName("").GetFathers().FindByRegex(@"\$@");