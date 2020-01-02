string[] typesMeth = new String[]{"filebuf","ofstream","fstream"};

CxList files = All.FindByTypes(typesMeth);

CxList exp = files.GetAncOfType(typeof(BinaryExpr));

result = All.FindByFathers(exp) - files;

CxList methods = Find_Methods();

List<string> outputMethods = new List<string>{		
		"fprintf",
		"fputs",
		"fputc",
		"fwrite"
		};

CxList f = methods.FindByShortNames(outputMethods); 

f.Add(methods.FindByMemberAccess("filebuf.sputc"));
f.Add(methods.FindByMemberAccess("filebuf.sputn")); 
f.Add(methods.FindByMemberAccess("ofstream.put"));
f.Add(methods.FindByMemberAccess("ofstream.write")); 
f.Add(methods.FindByMemberAccess("fstream.put"));
f.Add(methods.FindByMemberAccess("fstream.write"));

result.Add(f);