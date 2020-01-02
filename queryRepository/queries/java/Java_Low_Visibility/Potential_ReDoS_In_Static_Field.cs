// Find all evil strings
CxList evilStrings = Find_Evil_Strings();

// Sanitization
CxList sanitize = Find_Integers();

// Find all regex commands
CxList regex = All.FindByMemberAccess("Pattern.compile");

// Add static regexes (these do not influence their references, so needed here)
CxList staticFields = All.FindByFieldAttributes(Modifiers.Static);
staticFields = staticFields.FindByType(typeof(ConstantDecl)) + (staticFields * Find_Field_Decl());

result = evilStrings.InfluencingOnAndNotSanitized(regex.GetByAncs(staticFields), sanitize);