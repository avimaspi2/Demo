CxList read = All.FindByMemberAccess("StreamReader.Read*");
read.Add(All.FindByMemberAccess("Stream.Read*"));
read.Add(All.FindByMemberAccess("BinaryReader.Read*"));
read.Add(All.FindByMemberAccess("BufferedStream.Read*"));
read.Add(All.FindByMemberAccess("FileStream.Read*"));
read.Add(All.FindByMemberAccess("File.Read*"));
read.Add(All.FindByMemberAccess("MemoryStream.Read*"));
read.Add(All.FindByMemberAccess("UnmanagedMemoryStream.Read*"));
read.Add(All.FindByMemberAccess("StringReader.Read*"));
read.Add(All.FindByMemberAccess("PipeStream.Read*"));
read.Add(All.FindByMemberAccess("TextReader.Read*"));

// The web service methods will be considered as Read Methods
result.Add(read);
result.Add(Find_SP_Inputs());
result.Add(Find_Web_Services().GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr)));