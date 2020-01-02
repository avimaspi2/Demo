CxList comments = All.FindByRegex("#[^\n]*", CxList.CxRegexOptions.SearchInComments, RegexOptions.Singleline);

// All possible passwords
string regexTest = 
	@"\W(adgangskode|benutzerkennwort|chiffre|clave|codewort|contrasena|contrasenya|geheimcode|" + 
	@"geslo|heslo|jelszo|kennwort|losenord|losung|losungswort|lozinka|modpas|motdepasse|parol|" + 
	@"parola|parole|pasahitza|pasfhocal|passe|passord|passwort|password|pasvorto|paswoord|psw|pwd|" +
	@"salasana|schluessel|schluesselwort|senha|cifra|sifre|wachtwoord|wagwoord|watchword|zugangswort)";

// Possible delimiters that can go after a password
string regexPossibleDelimiters = @"(""|')?\s*([-:={[]+|is\s*('|"")|\([^)]*\)\s*:)";
// concatenate the full password search
string fullRegex = regexTest + regexPossibleDelimiters;


// Find passwords in comments
result = comments.FindByRegexExt(fullRegex, "*.py", true,
	CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);