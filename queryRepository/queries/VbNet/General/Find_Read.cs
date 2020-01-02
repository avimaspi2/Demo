CxList read = All.FindByMemberAccess("StreamReader.Read*", false);
read.Add(All.FindByMemberAccess("BinaryReader.Read*", false));
read.Add(All.FindByMemberAccess("BufferedStream.Read*", false));
read.Add(All.FindByMemberAccess("FileStream.Read*", false));
read.Add(All.FindByMemberAccess("MemoryStream.Read*", false));
read.Add(All.FindByMemberAccess("Stream.Read*", false));
read.Add(All.FindByMemberAccess("StreamReader.Read*", false));
read.Add(All.FindByMemberAccess("StringReader.Read*", false));
read.Add(All.FindByMemberAccess("TextReader.Read*", false));
read.Add(Find_SP_Inputs());

result = read;