// $ASP

CxList read =
	// Read, ReadAll, ReadLine
	Find_Member_With_Target_level2("Scripting.FileSystemObject", "OpenTextFile", "Read*") +
	Find_Member_With_Target_level2("Scripting.FileSystemObject", "CreateTextFile", "Read*") +

	// Read, ReadText
	Find_Member_With_Target("ADODB.Stream", "Read*");

CxList ldap = Find_LDAP().FindByShortName("attrvalue", false);

result = read + ldap;