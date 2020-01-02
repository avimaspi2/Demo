string fullRegex = @"(?<=\W)(password|psw|pwd|authKey|cipher|adgangskode|benutzerkennwort|chiffre|" +
	@"clave|codewort|contrasena|contrasenya|geheimcode|geslo|heslo|jelszo|kennwort|losenord|" +
	@"losung|losungswort|lozinka|modpas|motdepasse|parol|parola|parole|pasahitza|pasfhocal|" +
	@"passe|passord|passwort|pasvorto|paswoord|salasana|schluessel|schluesselwort|senha|sifre|" +
	@"wachtwoord|wagwoord|watchword|zugangswort|PAROLACHIAVE|PAROLA CHIAVE|PAROLECHIAVI|PAROLE CHIAVI|" +
	@"paroladordine|verschluesselt|sisma)(?=\W)(\s+[""'(]\w|\s*[:={[-])";

List <string> fileMaskList = new List<string> { "*.cbl"	};

// Find passwords in comments
result = All.FindByRegexExt(fullRegex, fileMaskList, true, CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);