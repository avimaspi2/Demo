CxList write = All.FindByMemberAccess("BinaryWriter.Write*");
write.Add(All.FindByMemberAccess("BufferedStream.Write*"));
write.Add(All.FindByMemberAccess("FileStream.Write*"));
write.Add(All.FindByMemberAccess("MemoryStream.Write*"));
write.Add(All.FindByMemberAccess("UnmanagedMemoryStream.Write*"));
write.Add(All.FindByMemberAccess("Stream.Write*"));
write.Add(All.FindByMemberAccess("StreamWriter.Write*"));
write.Add(All.FindByMemberAccess("StringWriter.Write*"));
write.Add(All.FindByMemberAccess("File.Write*"));
write.Add(All.FindByMemberAccess("PipeStream.Write*"));
write.Add(All.FindByMemberAccess("TextWriter.Write*"));

// All parameters of Web Services Methods is considered as Input methods
result.Add(write);
result.Add(Find_Log_Outputs());
result.Add(All.GetParameters(Find_Web_Services().GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr))));
result -= Find_Interactive_Inputs();