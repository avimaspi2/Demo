// CVE-2011-0447 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2011-0447
// Rails<2.3.11 : Cross-Site-Request-Forgery , CVE-2011-0447
// Corresponds to CWE-352 http://cwe.mitre.org/data/definitions/352.html
CxList ver = Find_Rails_Version();
result = Find_Gemlocks_Not_Satisfying_Version(ver, "2.3.11", "yes");