//this query checks if there is a web messaging output that broadcast a sensitive data
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
CxList temp = WebMessagingOutputOrigin * star;
temp.Add(WebMessagingOutputOrigin.DataInfluencedBy(star));
result = Web_Message_Outputs.FindByParameters(temp)
	.DataInfluencedBy(Find_Personal_Info());