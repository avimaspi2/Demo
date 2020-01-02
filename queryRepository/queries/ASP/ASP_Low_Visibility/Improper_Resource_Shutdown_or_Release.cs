CxList io = All.FindByType("*streamreader") +
			All.FindByType("*binaryreader") +
			All.FindByType("*bufferedstream") +
			All.FindByType("*filestream") +
			All.FindByType("*streamreader") +
			All.FindByType("*stringreader") +
			All.FindByType("*textreader") +	
			All.FindByType("*streamwriter") +
			All.FindByType("*binarywriter") +
			All.FindByType("*stringwriter") +
			All.FindByType("*textwriter");

CxList io_with_Close = All.FindAllReferences(All.FindByShortName("close").GetTargetOfMembers());

CxList Using = All.FindByType(typeof(UsingStmt));

CxList io_in_Using = All.GetByAncs(Using);

result = All.FindDefinition(io - io_with_Close - io_in_Using) - 
		 io.FindByType(typeof(ParamDecl));