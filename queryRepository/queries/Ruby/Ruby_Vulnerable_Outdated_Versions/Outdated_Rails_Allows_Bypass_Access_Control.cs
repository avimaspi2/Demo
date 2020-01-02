// CVE_2012_2660 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2012-2660
// Rails<3.2.4 : A remote attacker could send specially-crafted SQL statements using an unspecified parameter, 
//   which could allow the attacker to view, add, modify or delete information in the back-end database
// Corresponds CWE-264 to http://cwe.mitre.org/data/definitions/264.html
CxList ver = Find_Rails_Version();
result = Find_Gemlocks_Not_Satisfying_Version(ver, "3.2.4", "yes");

// CVE-2013-0155
// Ruby on Rails 3.0.x before 3.0.19, 3.1.x before 3.1.10, and 3.2.x before 3.2.11 does not properly consider 
//  differences in parameter handling between the Active Record component and the JSON implementation

result.Add(Find_Gemlocks_Not_Satisfying_Version(ver, "3.2.11", "yes"));