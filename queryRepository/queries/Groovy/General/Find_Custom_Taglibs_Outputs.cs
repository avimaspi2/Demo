/*
Call this method with 2 or 3 parameters:

Find_Custom_Taglibs_Outputs(<TAG-NAME>, <OUTPUT-ATTRIBUTE-NAME>, <ESCAPING-ATTRIBUTE-NAME>);
(last parameter is optional, if it's not there, we assume there is no sanitization)


1. The attribute value of "var" in the tag "ra:out" is an output,
   only when attribute "EscapeXSS" is set to false:

  result = Find_Custom_Taglibs_Outputs("ra:out", "var", "EscapeXSS"); 


2. The attribute value of "var" in the tag "ra:out" is an output,
   no matter if we have an escape false or not

  result = Find_Custom_Taglibs_Outputs("ra:out", "var");

*/

if (param.Length >= 2)
{
	try
	{
		string inTagName = param[0] as string;
		string inOutputVarName = param[1] as string;
		string inEscapeFalseName = "";
		if (param.Length > 2)
		{
			inEscapeFalseName = param[2] as string;
		}

		string tagName = inTagName.Replace(":", "_").Replace("-", "_") + "_*";
		if (inOutputVarName.Length > 0)
		{
			string outputVarName = "set" + inOutputVarName[0].ToString().ToUpper() + inOutputVarName.Substring(1);
			string escapeFalseName = "";
			if (inEscapeFalseName.Length > 0)
			{
				escapeFalseName = "set" + inEscapeFalseName[0].ToString().ToUpper() + inEscapeFalseName.Substring(1);
			}

			CxList relevantCustomTags = All.FindByShortName(tagName, false);
			CxList outputTags = relevantCustomTags;
			if (escapeFalseName != "")
			{
				CxList escapeXml = relevantCustomTags.GetMembersOfTarget().FindByShortName(escapeFalseName, false);
				CxList falseStrings = Find_Strings().FindByShortName("\"false\"", false);
				CxList escapeFalse = falseStrings.GetParameters(escapeXml);
				CxList potentialTags = All.FindByParameters(escapeFalse).GetTargetOfMembers();
				outputTags = relevantCustomTags.FindAllReferences(potentialTags);
			}
			CxList outputAttributes = outputTags.GetMembersOfTarget().FindByShortName(outputVarName, false);

			result = All.GetParameters(outputAttributes);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}