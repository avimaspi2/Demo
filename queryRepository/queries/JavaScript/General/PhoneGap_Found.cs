CxList listener = Find_UnknownReference().FindByShortName("document")
	.GetMembersOfTarget().FindByShortName("addEventListener");

result = Find_String_Literal().GetParameters(listener, 0).FindByShortName("deviceready");