// Query: Client_Negative_Content_Length
// The purpose of the query to detect using negative Content Lenght in XMLHttpRequest

CxList headers = Find_XHR_Headers();
CxList setRequestHeader = headers.FindByShortName("setRequestHeader");
CxList strings = Find_String_Literal();
CxList contentLength = strings.FindByShortNames(new List<string> {"CONTENT_LENGTH", "CONTENT-LENGTH"}, false);

CxList relevantSetHeader = setRequestHeader.FindByParameters(contentLength);
CxList xhrParams = All.GetParameters(relevantSetHeader, 1);
xhrParams -= Find_Parameters();
	
CxList elementsInAjax = All.GetParameters(Find_Methods().FindByMemberAccess("$.ajax")).FindByType(typeof(AssociativeArrayExpr));
CxList myFields = All.GetByAncs(elementsInAjax).FindByType(typeof(FieldDecl)).FindByShortName("headers");

CxList myContentLength = All.GetByAncs(myFields.GetByAncs(All)).FindByShortName("Content-Length");

xhrParams.Add(myContentLength.GetAssigner());
foreach(CxList prm in xhrParams)
{
	CSharpGraph p = prm.GetFirstGraph();
	// To be sure that p is not null
	if (p != null && p.FullName != null){
		try{
			string paramAsString = p.FullName;
			
			if (paramAsString.StartsWith("\"-") || paramAsString.StartsWith("'-"))
			{	
				result.Add(prm);
			}				
		}
		catch (Exception e)
		{
			cxLog.WriteDebugMessage("Client_Negative_Content_Length: build CSharapGrapf tested");
		}
	}
	
}