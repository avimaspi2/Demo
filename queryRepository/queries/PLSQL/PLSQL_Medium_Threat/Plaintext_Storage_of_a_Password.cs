// Plain Text Storage of Password

CxList passwords = Find_Passwords();
CxList sanitize = Find_General_Sanitize(); 

CxList inputs = Find_Read() + Find_Inputs();

result = passwords.InfluencedByAndNotSanitized(inputs, sanitize);