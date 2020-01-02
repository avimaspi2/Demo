/*
see: https://www.securecoding.cert.org/confluence/display/perl/OOP32-PL.+Prohibit+indirect+object+call+syntax

The 'indirect object call syntax' is a grammatical mechanism used by Perl to parse method calls.
It is commonly used to emulate other language syntax. For instance, if a class Class has a constructor
named new, then both of these statements invoke this constructor:
  my $obj1 = Class->new; # 'object-oriented' syntax
  my $obj = new Class; # 'indirect object' syntax
The [perlobj manpage] states the following:
   The -> notation suffers from neither of these disturbing ambiguities, so we recommend you use it exclusively.
   However, you may still end up having to read code using the indirect object notation, so it's important to be
   familiar with it.

Consequently, indirect object syntax shall not be used.
*/

result = All.FindByType(typeof(Declarator)).FindByRegex(@"=\s*new\s+\$?\w");