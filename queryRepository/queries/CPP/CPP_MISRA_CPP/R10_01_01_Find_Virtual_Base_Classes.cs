/*
 MISRA CPP RULE 10-1-1
 ------------------------------
 This query searches for classes derived from virtual classes.
 
 The Example below shows code with vulnerability: 

      class Foo
			{
			public:
				void DoSomething() {  }
			};
		class Bar : public virtual Foo 			//Non-compliant
			{
			public:
			    void DoSpecific() { }
			};

*/

CxList classes = All.FindByType(typeof(ClassDecl));
foreach(CxList curr in classes) {

	ClassDecl currClass = (ClassDecl) curr.data.GetByIndex(0);
	TypeRefCollection bases = currClass.BaseTypes;
	foreach (TypeRef currType in bases) {
		String name = currType.TypeName;
		if (name.Equals("virtual")) {
			result.Add(curr);
			continue;
		}
	}
}