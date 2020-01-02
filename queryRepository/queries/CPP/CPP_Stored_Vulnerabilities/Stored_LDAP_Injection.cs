////////////////////////////////////////////////////
//LDAP Injection is an attack used to exploit web based
//applications that construct LDAP statements based on stored inputs.
//https://www.owasp.org/index.php/LDAP_injection
////////////////////////////////////////////////////

//Find potentially malicious content from stored files or databases
CxList stored = Find_DB();
stored.Add(Find_Read());

//Find LDAP sanitizers
CxList sanitize = Find_LDAP_Sanitize();

//Find LDAP outputs
CxList outputs = Find_LDAP_Outputs();

result = outputs.InfluencedByAndNotSanitized(stored, sanitize);