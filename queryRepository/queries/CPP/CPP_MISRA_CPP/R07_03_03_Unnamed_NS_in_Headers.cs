/*
 MISRA CPP RULE 7-3-3
 ------------------------------
 This query searches for empty namespaces in header files.

 The Example below shows code with vulnerability: 

	  //file.h
      namespace //Non-compliant
		{
			extern int x;
		}

*/

CxList heads = All.FindByFileName("*.h") +
	All.FindByFileName("*.hpp") +
	All.FindByFileName("*.hh");
CxList nss = heads.FindByType(typeof(NamespaceDecl));
foreach(CxList ns in nss) {
	String name = ((NamespaceDecl) ns.data.GetByIndex(0)).Name;
	if(name.Contains("CX_NAMESPACE")) {
		result.Add(ns);
	}
}