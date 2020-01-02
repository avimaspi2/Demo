if(All.isWebApplication)
{
	result = All.FindByMemberAccess("TcpListener.Accept", false);	
	result.Add(All.FindByMemberAccess("TcpListener.AcceptSocket", false));	
	result.Add(All.FindByMemberAccess("TcpListener.AcceptSocketAsync", false));	
	result.Add(All.FindByMemberAccess("TcpListener.AcceptTcpClient", false));	
	result.Add(All.FindByMemberAccess("TcpListener.AcceptTcpClientAsync", false));	
}