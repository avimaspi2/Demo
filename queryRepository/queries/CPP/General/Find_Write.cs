CxList files = All.FindByType("filebuf") + 
	All.FindByType("ofstream") + 
	All.FindByType("fstream");
CxList exp = files.GetAncOfType(typeof(BinaryExpr));
exp.Add(files.GetAncOfType(typeof(AssignExpr)));

result = All.FindByFathers(exp) - files;

CxList methods = Find_Methods();

CxList f = methods.FindByShortName("fprintf") + 
	methods.FindByShortName("fputs") + 	
	methods.FindByShortName("fputc") + 	
	methods.FindByShortName("fwrite");

f.Add(methods.FindByMemberAccess("filebuf.sputc") +
	methods.FindByMemberAccess("filebuf.sputn") + 
	methods.FindByMemberAccess("ofstream.put") + 
	methods.FindByMemberAccess("ofstream.write") + 
	methods.FindByMemberAccess("fstream.put") + 
	methods.FindByMemberAccess("fstream.write"));

result.Add(f);