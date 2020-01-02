CxList identifiers = 
	Find_All_Declarators() +
	All.FindByType(typeof(MemberDecl)) +
	Find_ParamDecl();

// remove field decls who are counter by their declarator already
identifiers -= identifiers.FindByType(typeof(Declarator)).GetAncOfType(typeof(FieldDecl));

// remove constructor decls
identifiers -= identifiers.FindByType(typeof(ConstructorDecl));
identifiers -= identifiers.FindByType(typeof(DestructorDecl));


// remove artificially added identifiers
identifiers -= (identifiers.FindByShortName("*checkmarx_default*") +
	identifiers.FindByShortName("INCLUDEREPLACE*") +
	identifiers.FindByShortName("CX_INCLUDE*") +
	identifiers.FindByShortName(""));

result = identifiers;