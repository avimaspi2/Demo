// CVE-2012-3464 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2012-3464
// Cross-site scripting (XSS) vulnerability in in Ruby on Rails before 3.0.17, 3.1.x before 3.1.8, and 3.2.x before 3.2.8 might
//   allow remote attackers to inject arbitrary web script or HTML via vectors involving a ' (quote) character. 
// Corresponds to CWE 79, http://cwe.mitre.org/data/definitions/79.html

// Not escaping single quotes
CxList ver = Find_Rails_Version();
result = Find_Gemlocks_Not_Satisfying_Version(ver, "3.2.8", "yes");

// Rails<3.0.11 : XSS- vulnerability in the translate helper keys may allow 
//   an attacker to insert arbitrary code into a page
// http://groups.google.com/group/rubyonrails-security/browse_thread/thread/2b61d70fb73c7cc5

result.Add(Find_Gemlocks_Not_Satisfying_Version(ver, "3.0.11", "yes"));