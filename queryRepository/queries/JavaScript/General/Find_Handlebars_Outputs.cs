/*
This query is searching for methods named "CxOutput" from Handlebars
*/

if(cxScan.IsFrameworkActive("Handlebars"))
{
	//1 - Find CxOutput inside HBTemplate Declarations
	CxList hbTemplates = Find_MethodDecls().FindByShortName("*HBTemplate");
	CxList hbTemplatesOutputs = Find_Framework_Outputs().GetByAncs(hbTemplates);
	
	//2 - Add results
	result = hbTemplatesOutputs;
}