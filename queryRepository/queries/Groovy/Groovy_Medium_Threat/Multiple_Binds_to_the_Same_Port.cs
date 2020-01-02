CxList bind = All.FindByMemberAccess("ServerSocket.bind");
CxList socketAddress = All.FindByType("InetSocketAddress");
CxList ports = All.GetParameters(socketAddress).FindByType(typeof(IntegerLiteral));
CxList portsMethods = All.GetMethod(ports);

foreach (CxList method in portsMethods)
{
	CxList methodPorts = ports.GetByAncs(method);
	SortedList portNumbers = new SortedList();
	foreach(CxList port in methodPorts)
	{
		IntegerLiteral g = port.data.GetByIndex(0) as IntegerLiteral;
		int portNumber = -1;
		if (int.TryParse(g.ShortName, out portNumber))
		{
			if (portNumbers.ContainsValue(portNumber))
			{
				result.Add(g.NodeId, g);
				break;
			}
			else
			{
				portNumbers[portNumber] = portNumber;
			}
		}
	}
}