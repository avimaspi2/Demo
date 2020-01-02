CxList Web_Message_Outputs = Find_Web_Messaging_Outputs();
CxList WebMessagingOutputOrigin = All.GetParameters(Web_Message_Outputs, 1);

CxList strings = Find_String_Literal();
CxList star = All.NewCxList();
foreach(CxList str in strings)
{
	CSharpGraph g = str.GetFirstGraph();
	if(g != null && g.ShortName != null)
	{
		string name = g.ShortName;
		if(name.Equals("\"*\""))
		{
			star.Add(str);
		}
	}
}
result = WebMessagingOutputOrigin * star;