//Find DB Linq Access

string[] dbCommit = new string[] {
	"DeleteOnSubmit",
	"InsertOnSubmit",
	"UpdateOnSubmit",
	"SubmitChanges",
	"OnSubmit",
	"Select",
	"SelectMany"	
	};

CxList methods = Find_Methods();
List < string > linqMethods = new List<string>() {
		"GroupBy",
		"Join",
		"OrderBy",
		"Select",
		"SelectMany",
		"Where"
		};
//Look for all methods that calling CxLINQ()
CxList linqList = methods.FindByShortNames(linqMethods)
	.GetByAncs(methods.FindByShortName("CxLINQ"));

linqList = linqList.GetTargetOfMembers();
linqList.Add(All.GetParameters(linqList));
	
//In following block looking for all new() operation 
//with result of DataContext type
// Output example: 
// 1) Dim dc As New MyDataClassesDataContext()
// 2) Dim dbCl As DataClasses3DataContext = New MyDataClasses3DataContext()
CxList objCreated = All.FindByFathers(All.FindByType(typeof(ObjectCreateExpr)));
//CxList objCreatedByDCType = objCreated.FindByType("*DataContext*") + objCreated.InheritsFrom("*DataContext*");
CxList objCreatedByDCType = objCreated.FindByType("*DataContext*"); 
objCreatedByDCType.Add(objCreated.InheritsFrom("*DataContext*"));

CxList decl = objCreatedByDCType.GetAncOfType(typeof(Declarator));
CxList varByDCType = All.FindAllReferences(decl);
varByDCType.Add(varByDCType.GetMembersOfTarget());
CxList varDCType = All.FindByType("*DataContext*");
CxList varRef = All.FindAllReferences(varDCType);

// Block below looks for LINQ() methods where parameter data context type 
// Dim X = LINQ(product)
linqList = linqList.DataInfluencedBy(varByDCType + varRef + varRef.GetMembersOfTarget());

// Took left side of the assignment
// In example above the X variable is represent the access to DB
CxList linq = All.GetByAncs(linqList.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
linq.Add(All.GetByAncs(linqList.GetAncOfType(typeof(VariableDeclStmt))).FindByAssignmentSide(CxList.AssignmentSide.Left));

//Block below looks for direct DB acccess by X variable
//for example:
// x.OnSubmit();
CxList linqRef = All.FindAllReferences(linq);
CxList linqTarget = linqRef.GetMembersOfTarget();
CxList directDBAccess = All.NewCxList();

directDBAccess.Add(linqTarget.FindByShortNames(new List<string> (dbCommit), false));
//foreach (string str in dbCommit)
//{
//	directDBAccess.Add(linqTarget.FindByShortName(str, false));
//}

//If X is returned value, it may be used after it
//we are looking who call the function that returns X
//Example z = foo()
// foo(){ Dim X = LINQ(product); Return X}

CxList returnStmt = linqRef.GetAncOfType(typeof(ReturnStmt));// looks Anc that returns X
CxList returnStmtAncMethod = returnStmt.GetAncOfType(typeof(MethodDecl)); // Tooks just functions it self: foo()
CxList refDBAccessMethods = All.FindAllReferences(returnStmtAncMethod) - returnStmtAncMethod; //looks for z = foo()
//Build the methdos that applied 
// For example: 	Accessor.GetTopFiveOrdersById(OrderPosition).InsertOnSubmit(cust)
CxList theTarget = refDBAccessMethods.GetMembersOfTarget();
CxList DBAccessRightSide = All.NewCxList();
DBAccessRightSide.Add(theTarget.FindByShortNames(new List<string> (dbCommit), false));
//foreach (string str in dbCommit)
//{
//	DBAccessRightSide.Add(theTarget.FindByShortName(str, false));
//}


// The block below looks for "second oreder"
// For example
// 1) 	Dim xxx
//		xxx = GetOrderById(1)
//		xxx.DeleteOnSubmit(cust)
//
// 2)  dataGridView1.DataSource = Accessor.GetTopFiveOrdersById(OrderPosition)	
//	   dataGridView1.DataSource.InsertOnSubmit(cust)

CxList secondOrderLinq = All.GetByAncs(refDBAccessMethods.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList secondOrderLinqRef = All.FindAllReferences(secondOrderLinq);
CxList secondOrderLinqTarget = secondOrderLinqRef.GetMembersOfTarget();

CxList secondOrderDBAccess = All.NewCxList();
secondOrderDBAccess.Add(secondOrderLinqTarget.FindByShortNames(new List<string> (dbCommit), false));

// The last block looks for access to DataContext methods listed below, without any connection to 

CxList linqAccess = All.FindByMemberAccess("*DataContext*.DeleteOnSubmit");
linqAccess.Add(All.FindByMemberAccess("*DataContext*.InsertOnSubmit"));
linqAccess.Add(All.FindByMemberAccess("*DataContext*.UpdateOnSubmit"));
linqAccess.Add(All.FindByMemberAccess("*DataContext*.SubmitChanges"));
linqAccess.Add(All.FindByMemberAccess("*DataContext*.OnSubmit"));
linqAccess.Add(All.FindByMemberAccess("*DataContext*.Select"));
linqAccess.Add(All.FindByMemberAccess("*DataContext*.SelectMany"));

result = secondOrderDBAccess;
result.Add(DBAccessRightSide);
result.Add(directDBAccess);
result.Add(linqAccess);