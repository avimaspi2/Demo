// All possible passwords
string regexTest = 
	@"(?<=\W)(adgangskode|benutzerkennwort|chiffre|clave|codewort|contrasena|contrasenya|geheimcode|" + 
	@"geslo|heslo|jelszo|kennwort|losenord|losung|losungswort|lozinka|modpas|motdepasse|parol|" + 
	@"parola|parole|pasahitza|pasfhocal|passe|passord|passwort|password|pasvorto|paswoord|psw|pwd|" +
	@"salasana|schluessel|schluesselwort|senha|sifre|wachtwoord|wagwoord|watchword|zugangswort)(?=(\s|,|""|')*(=|:))";

List <string> fileMaskList = new List<string>
	{
		"*.swift", "*.h", "*.m",
	};
// Find passwords in comments
result = All.FindByRegexExt(regexTest, fileMaskList, true, CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);