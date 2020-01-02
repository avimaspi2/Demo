// CVE-2013-0269  http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2013-0269
// The JSON gem before 1.5.5, 1.6.x before 1.6.8, and 1.7.x before 1.7.7 for Ruby allows remote attackers ...
// Corresponds to CWE-20 http://cwe.mitre.org/data/definitions/20.html

result = Find_Packages_Not_Satisfying_Version("json", "1.7.7");