List <string> fileMaskList = new List<string>{"*.java", "*.jsp", "*.jspf", "*jsf", "*.vm"};

// Possible delimiters that can go after a password - we only want passwords in formets like password: 45iy345
string potential_problem = @"\s+[""']\w";
string dafinite_problem = @"\s*[:={[-]";
string not_passwords = @"\s*((-?)->)";

// All possible passwords
string full_regex = @"(?<=\W)(?<=[^""'])(password|psw|pwd|authKey|cipher|adgangskode|benutzerkennwort|chiffre|clave|" +
					@"codewort|contrasena|contrasenya|geheimcode|geslo|heslo|jelszo|kennwort|losenord|losung|losungswort|" + 
					@"lozinka|modpas|motdepasse|parol|parola|parole|pasahitza|pasfhocal|passe|passord|passwort|pasvorto|" + 
					@"paswoord|salasana|schluessel|schluesselwort|senha|sifre|wachtwoord|wagwoord|watchword|zugangswort|" + 
					@"PAROLACHIAVE|PAROLA CHIAVE|PAROLECHIAVI|PAROLE CHIAVI|paroladordine|verschluesselt|sisma)";

full_regex += "(?!" + not_passwords + ")(?=" + dafinite_problem + "|" + potential_problem + ")";

// Find passwords in comments
result = All.FindByRegexExt(full_regex, fileMaskList, true, CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);