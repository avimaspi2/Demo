CxList perlCommands = Find_Perl_Commands();

result = 
	perlCommands.FindByType(typeof(MethodDecl)) +
	perlCommands.FindByType(typeof(ClassDecl));