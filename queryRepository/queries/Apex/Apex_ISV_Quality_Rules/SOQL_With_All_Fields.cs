string[] allFields = { "createddate", "isdeleted", "lastmodifiedbyid",
	"lastmodifieddate", "systemmodstamp"}; 

result = Extract_From_SOQL("select", "createdbyid");
foreach (string curField in allFields)
{
	result *= Extract_From_SOQL("select", curField);
}