List<string> linqSeparators = new List<string> {
	"aggregate",
	"all",
	"allasync",
	"any",
	"anyasync",
	"ascending",
	"asenumerable",
	"average",
	"averageasync",
	"cast",
	"concat",
	"contains",
	"containsasync",
	"count",
	"countasync",
	"defaultifempty",
	"descending",
	"distinct",
	"elementat",
	"elementatordefault",
	"empty",
	"except",
	"first",
	"firstasync",
	"firstordefault",
	"firstordefaultaync",
	"foreach",
	"foreachasync",
	"groupby",
	"groupjoin",
	"intersect",
	"include",
	"join",
	"last",
	"load",
	"loadasync",
	"lastasync",
	"lastordefault",
	"lastordefaultasync",
	"longcount",
	"longcountasync",
	"max",
	"maxasync",
	"min",
	"minasync",
	"oftype",
	"orderby",
	"orderbydescending",
	"range",
	"repeat",
	"reverse",
	"select",
	"selectmany",
	"sequenceequal",
	"single",
	"singleasync",
	"singleordefault",
	"singleordefaultasync",
	"skip",
	"skipwhile",
	"sum",
	"sumasync",
	"take",
	"takewhile",
	"thenby",
	"thenbydescending",
	"theninclude",
	"toarray",
	"toarrayasync",
	"todictionary",
	"todictionaryasync",
	"tolist",
	"tolistasync",
	"tolookup",
	"union",
	"where",
    "zip",
	
	"DeleteOnSubmit",
	"InsertOnSubmit",
	"UpdateOnSubmit",
	"SubmitChanges",
	"OnSubmit"	
	};
string[] types = new string[]{
	"DataContext", "DbContext", "DbQuery", "DbSet", 
	"ObjectQuery","ObjectContext", "ObjectSet"};

//All linq classes
CxList inherits = All.NewCxList();	
foreach(string type in types){
	inherits.Add(All.InheritsFrom(type));
}

//All linq objects
CxList linq = All.FindByTypes(types);
linq.Add(inherits);

CxList childs = All.FindByFathers(All.FindByType(typeof(ObjectCreateExpr)));
CxList linq1 = childs.FindByTypes(types);
linq1.Add(childs * inherits);

linq.Add(All.GetByAncs(linq1.GetAncOfType(typeof(AssignExpr)))
	.FindByAssignmentSide(CxList.AssignmentSide.Left));
linq.Add(linq1.GetAncOfType(typeof(Declarator)));

linq = All.FindAllReferences(linq);

linq = Find_All_Members(linq);
linq -= linq.FindByShortNames(linqSeparators, false);

CxList tables = All.FindByType("Table");
// Leave only the ones that are declared as Table.
// Otherwise it might be the word "Table", but with a different type
tables = tables.FindAllReferences(All.FindDefinition(tables));

// remove all results with Table.rows and Table.columns
CxList removeResult = All.FindByMemberAccess("Table.Columns") + All.FindByMemberAccess("Table.Rows")
	- All.FindByMemberAccess("DataTable.Columns") - All.FindByMemberAccess("DataTable.Rows");

removeResult = removeResult.GetTargetOfMembers();
tables = tables - removeResult;

linq.Add(tables);

CxList linqMethods = Find_Methods().FindByParameters(linq);
linqMethods.Add(linqMethods.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)));
linqMethods.Add(linqMethods.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)));
linqMethods.Add(linqMethods.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)));

// select only methods of linq (appears in linqSeparators)
result.Add(linqMethods.FindByShortNames(linqSeparators, false));