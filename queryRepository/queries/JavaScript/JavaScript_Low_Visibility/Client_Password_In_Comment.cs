string full_regex = @"(?<=\W)(?<=[^""'])(password|psw|pwd|authKey|cipher|pass|adgangskode|benutzerkennwort|chiffre|" + 
	@"clave|codewort|contrasena|contrasenya|geheimcode|geslo|heslo|jelszo|kennwort|losenord|losung|losungswort|lozinka|" +
	@"modpas|motdepasse|parol|parola|parole|pasahitza|pasfhocal|passe|passord|passwort|pasvorto|paswoord|salasana|" +
	@"schluessel|schluesselwort|senha|sifre|wachtwoord|wagwoord|watchword|zugangswort|PAROLACHIAVE|PAROLA CHIAVE|" + 
	@"PAROLECHIAVI|PAROLE CHIAVI|paroladordine|verschluesselt|sisma)(?=\W)\s*([""':={[|-]|(is[\s""':=({[|-])|\((?!\)))";

List <string> fileMaskList = new List<string>
	{
		"*.js", "*.asp", /*"*.html", "*.htm",*/"*.aspx",
		"*.hbs","*.handlebars","*.jade","*.pug", "*.ts", "*.tsx"
		};

// Find passwords in comments
result = All.FindByRegexExt(full_regex, fileMaskList, true, CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);