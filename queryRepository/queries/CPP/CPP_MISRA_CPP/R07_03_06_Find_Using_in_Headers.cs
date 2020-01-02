/*
 MISRA CPP RULE 7-3-6
 ------------------------------
 This query searches usage of using-directives or using-declarations within header files.
 
 The Example below shows code with vulnerability: 

	//file.h
	namespace NS {
		extern int i;
	}
	using namespace NS; 	//Non-compliant
	using NS::i; 			//Non-compliant

*/

CxList heads = All.FindByFileName("*.h") +
	All.FindByFileName("*.hpp") +
	All.FindByFileName("*.hh");
CxList imports = heads.FindByType(typeof(Import));
result = imports.FindByRegex(@"using/s", false, false,false);