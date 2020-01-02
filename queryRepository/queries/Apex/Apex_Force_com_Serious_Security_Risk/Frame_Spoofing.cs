CxList inputs = Find_Interactive_Inputs() - Find_Url_Current_Page();
CxList iframe = Find_VF_Pages().FindByMemberAccess("iframe.src");
iframe = Find_VF_Pages().GetByAncs(iframe) - iframe;

CxList sanitize = Find_Integers() + Find_Id_Sanitizers() + Find_Test_Code() + Find_NonLeft_Binary(iframe);

result = Find_VF_O().InfluencedByAndNotSanitized(iframe, sanitize).InfluencedByAndNotSanitized(inputs, sanitize);

result -= Find_Test_Code();