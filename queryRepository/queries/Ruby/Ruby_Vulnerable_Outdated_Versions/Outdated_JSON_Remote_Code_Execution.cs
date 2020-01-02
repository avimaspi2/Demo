// CVE-2013-0333 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2013-0333
// Rails<3.0.20 : lib/active_support/json/backends/yaml.rb in Ruby on Rails 2.3.x before 2.3.16 and 3.0.x 
//    before 3.0.20 does not properly convert JSON data to YAML data[...] allows remote attackers to execute arbitrary code
// Corresponds to CWE 94, http://cwe.mitre.org/data/definitions/94.html


CxList ver = Find_Rails_Version();
result = Find_Gemlocks_Not_Satisfying_Version(ver, "3.0.20", "yes");