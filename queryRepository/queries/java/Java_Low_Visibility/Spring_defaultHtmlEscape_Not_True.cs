CxList webxml = Find_Web_Xml();
bool isSpring = webxml.FindByShortName("*spring*", false).Count > 0;

if (isSpring)
{
	CxList encoding = webxml.FindByShortName(@"""defaultHtmlEscape""");
	CxList pos = webxml.FindByShortName(@"""true""");

	CxList defaultEscaping = webxml.FindByShortName("CONTEXT_PARAM").DataInfluencedBy(encoding);
	defaultEscaping = webxml.FindAllReferences(defaultEscaping);
	defaultEscaping = pos.DataInfluencingOn(defaultEscaping);

	if (defaultEscaping.Count == 0)
	{
		result = Find_Class_Decl().FindByShortName("CxXmlConfigClass*") * webxml;
	}
}