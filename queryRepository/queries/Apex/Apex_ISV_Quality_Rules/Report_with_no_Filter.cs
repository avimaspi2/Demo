//Finds report metadata with no filters
CxList reportsCode = All.FindByFileName("*.report");
CxList reports = reportsCode.FindByType(typeof(ClassDecl));
result = reports - reportsCode.FindByMemberAccess("report.filter").GetAncOfType(typeof(ClassDecl));