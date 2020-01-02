CxList personal = Find_String_Short_Name(All, new List<string>{
		"*Account*","*credentials*","*Credit*","*secret*",
		"*SocialSecurity*","*SSN","SSN*","dob",/*"user","userName",*/
		"auth*"}, false);

//specific list of keywords that don't disclose personal info.
List < string > whitelist = new List< string >
	{"author", "author_*", "authors", "authors_*" ,"authorurl", 
		"authoruri", "*className*", "authorElt", "authority"};
 

personal -= Find_String_Short_Name(All, whitelist, false);

result = All_Passwords();

foreach (CxList p in personal){
	CSharpGraph g = p.GetFirstGraph();
	if(g != null && g.ShortName != null && 
	g.ShortName.Length < 20){
		result.Add(p);
	}
}