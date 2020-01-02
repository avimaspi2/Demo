// CVE-2011-2930 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2011-2930
// Multiple SQL injection vulnerabilities in the quote_table_name method in the ActiveRecord adapters in activerecord/lib/active_record/connection_adapters/ in Ruby on Rails before 2.3.13, 3.0.x before 3.0.10, and 3.1.x before 3.1.0.rc5 allow remote attackers to execute arbitrary SQL commands via a crafted column name. 
// Corresponds to CWE-89 http://cwe.mitre.org/data/definitions/89.html
CxList ver = Find_Rails_Version();
result = Find_Gemlocks_Not_Satisfying_Version(ver, "3.1.0.rc5", "yes");

// CVE-2012_2695 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2012-2695
// The Active Record component in Ruby on Rails before 3.0.14, 3.1.x before 3.1.6, and 3.2.x before 3.2.6 does not properly implement the passing of request data to a where method in an ActiveRecord class, which allows remote attackers to conduct certain SQL injection attacks via nested query parameters that leverage improper handling of nested hashes, a related issue to CVE-2012-2661. 
// Rails<3.2.6 : SQL Injection: CVE-2012-2695
// Corresponds to CWE-89 http://cwe.mitre.org/data/definitions/89.html
result.Add(Find_Gemlocks_Not_Satisfying_Version(ver, "3.2.6", "yes"));

// CVE-2012-6496 http://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2012-6496
// Rails<3.2.10 : SQL Injection: CVE-2012-6496
// SQL injection vulnerability in the Active Record component in Ruby on Rails before 3.0.18, 3.1.x before 3.1.9, and 3.2.x before 3.2.10 allows remote attackers to execute arbitrary SQL commands via a crafted request that leverages incorrect behavior of dynamic finders in applications that can use unexpected data types in certain find_by_ method calls. 

result.Add(Find_Gemlocks_Not_Satisfying_Version(ver, "3.2.10", "yes"));