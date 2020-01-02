CxList io = All.FindByType("*StreamReader", false);
io.Add(All.FindByType("*BinaryReader", false));
io.Add(All.FindByType("*BufferedStream", false));
io.Add(All.FindByType("*FileStream", false));
io.Add(All.FindByType("*StreamReader", false));
io.Add(All.FindByType("*StringReader", false));
io.Add(All.FindByType("*TextReader", false));
io.Add(All.FindByType("*StreamWriter", false));
io.Add(All.FindByType("*BinaryWriter", false));
io.Add(All.FindByType("*StringWriter", false));
io.Add(All.FindByType("*TextWriter", false));

CxList io_with_Close = All.FindAllReferences(All.FindByShortName("Close", false).GetTargetOfMembers());

CxList Using = All.FindByType(typeof(UsingStmt));

CxList io_in_Using = All.GetByAncs(Using);

result = All.FindDefinition(io - io_with_Close - io_in_Using) - 
		 io.FindByType(typeof(ParamDecl));