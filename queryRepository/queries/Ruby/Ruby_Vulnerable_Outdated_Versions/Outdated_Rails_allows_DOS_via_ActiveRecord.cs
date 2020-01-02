// CVE-2013-1854 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2013-1854
// Rails<3.2.13: Denial of Service via ActiveRecord
// Corresponds to CWE-400 http://cwe.mitre.org/data/definitions/400.html
CxList ver = Find_Rails_Version();
result = Find_Gemlocks_Not_Satisfying_Version(ver, "3.2.13", "yes");