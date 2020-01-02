/*
Deprecated		Preferred
----------		---------
die()			Carp::croak()
warn()	 		Carp::carp()
format()		Template, Perl6::Form
realpath()		CWD::abs_path
*/

CxList methods = Find_Methods();

result = 
	methods.FindByShortName("die") +
	methods.FindByShortName("warn") +
	methods.FindByShortName("format") +
	methods.FindByShortName("realpath");