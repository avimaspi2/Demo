// This query determines if the program is a CGI application. We can conclude it is a CGI application if:

/*	The getenv method accesses to CGI environment variable "QUERY_STRING" 
		getenv("QUERY_STRING")	*/
CxList getenv = Find_Environment_Inputs();
CxList queryString = Find_Strings().GetParameters(getenv).FindByShortName("QUERY_STRING");

/* 	There is a line printing "Content-type:text/html\r\n\r\n". This line is sent back to the browser and specify 
	the content type to be displayed on the browser screen (in the example is html but can be another extension)
		cout << "Content-type:text/html\r\n\r\n";	*/
CxList contentType = Find_Strings().FindByShortName("Content-type*", false);

/* When library cgicc is used - https://www.gnu.org/software/cgicc/. Therefore we try to find a class Cgicc declaration
		Cgicc cgi	*/
CxList cgicc = Find_All_Declarators().FindByType("Cgicc");

result = queryString;
result.Add(contentType);
result.Add(cgicc);