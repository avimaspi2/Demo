////////////////////////////////////////////////////
//LDAP Injection is an attack used to exploit web based
//applications that construct LDAP statements based on user input.
//https://www.owasp.org/index.php/LDAP_injection
////////////////////////////////////////////////////

//Find all interactive inputs
CxList inputs = Find_Interactive_Inputs();

//Find LDAP sanitizers
CxList sanitize = Find_LDAP_Sanitize();

//Find LDAP outputs
CxList outputs = Find_LDAP_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);