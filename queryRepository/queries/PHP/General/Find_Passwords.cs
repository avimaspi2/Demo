CxList allStrings = All;

CxList tempResult = allStrings.FindByShortNames(new List<String>()
	{ "*password*", "*psw", "psw*", "pwd*", "*pwd", "pass*" }, false);

// Remove words from dictionary
tempResult -= tempResult.FindByShortNames(new List<String>()
	{ "*pass", "*passable*", "*passage*", "*passenger*", "*passer*", "*passing*", "*passion*", 
		"*passive*", "*passover*", "*passport*", "*passed*", "*compass*", "*bypass*" }, false);

List<String> passwordStrings = new List<String> {
		"dbpass",
		"pass",
		"adgangskode",// Danish
		"benutzerkennwort",// German
		"chiffre", 	// German
		"clave", 		// Spanish
		"codewort", 	// German
		"contrasena", 	// Spanish
		"contrasenya", 	// Catalan
		"geheimcode", 	// German
		"geslo", 		// Slovenian
		"heslo ", 		// Czech
		"jelszo", 		// Hungarian
		"kennwort", 	// German
		"losenord", 	// Swedish
		"losung", 		// German
		"losungswort", 	// German
		"lozinka", 		// Bosnian, Croatian
		"modpas", 		// Haitian-creole
		"motdepasse", 	// French
		"parol", 		// Azerbaijani
		"parola", 		// Russian
		"parole", 		// German
		"pasahitza", 	// Basque
		"pasfhocal", 	// Irish
		"passe", 		// French
		"passord", 		// Norwegian
		"passwort", 	// Spanish
		"pasvorto", 	// Esperanto
		"paswoord", 	// Dutch
		"salasana", 	// Finnish
		"schluessel", 	// German
		"schluesselwort", // German
		"senha", 		// Portuguese
		"sifre", 		// Turkish
		"wachtwoord", 	// Dutch
		"wagwoord", 	// Afrikaans
		"watchword", 	// English
		"zugangswort", 	// German
		"PAROLACHIAVE", // Italian
		"PAROLA CHIAVE", // Italian
		"PAROLECHIAVI", // Italian
		"PAROLE CHIAVI", // Italian
		"paroladordine", // Spanish
		"verschluesselt", // German
		"sisma" // Hebrew
		};

tempResult.Add(allStrings.FindByShortNames(passwordStrings, false));
/*
tempResult.Add(allStrings.FindByShortName("pass", false));
tempResult.Add(allStrings.FindByShortName("adgangskode", false));	// Danish
tempResult.Add(allStrings.FindByShortName("benutzerkennwort", false));// German
tempResult.Add(allStrings.FindByShortName("chiffre", false));		// German
tempResult.Add(allStrings.FindByShortName("clave", false));			// Spanish
tempResult.Add(allStrings.FindByShortName("codewort", false));		// German
tempResult.Add(allStrings.FindByShortName("contrasena", false));	// Spanish
tempResult.Add(allStrings.FindByShortName("contrasenya", false));	// Catalan
tempResult.Add(allStrings.FindByShortName("geheimcode", false));	// German
tempResult.Add(allStrings.FindByShortName("geslo", false));			// Slovenian
tempResult.Add(allStrings.FindByShortName("heslo ", false));		// Czech
tempResult.Add(allStrings.FindByShortName("jelszo", false));		// Hungarian
tempResult.Add(allStrings.FindByShortName("kennwort", false));		// German
tempResult.Add(allStrings.FindByShortName("losenord", false));		// Swedish
tempResult.Add(allStrings.FindByShortName("losung", false));		// German
tempResult.Add(allStrings.FindByShortName("losungswort", false));	// German
tempResult.Add(allStrings.FindByShortName("lozinka", false));		// Bosnian, Croatian
tempResult.Add(allStrings.FindByShortName("modpas", false));		// Haitian-creole
tempResult.Add(allStrings.FindByShortName("motdepasse", false));	// French
tempResult.Add(allStrings.FindByShortName("parol", false));			// Azerbaijani
tempResult.Add(allStrings.FindByShortName("parola", false));		// Russian
tempResult.Add(allStrings.FindByShortName("parole", false));		// German
tempResult.Add(allStrings.FindByShortName("pasahitza", false));		// Basque
tempResult.Add(allStrings.FindByShortName("pasfhocal", false));		// Irish
tempResult.Add(allStrings.FindByShortName("passe", false));			// French
tempResult.Add(allStrings.FindByShortName("passord", false));		// Norwegian
tempResult.Add(allStrings.FindByShortName("passwort", false));		// Spanish
tempResult.Add(allStrings.FindByShortName("pasvorto", false));		// Esperanto
tempResult.Add(allStrings.FindByShortName("paswoord", false));		// Dutch
tempResult.Add(allStrings.FindByShortName("salasana", false));		// Finnish
tempResult.Add(allStrings.FindByShortName("schluessel", false));	// German
tempResult.Add(allStrings.FindByShortName("schluesselwort", false));// German
tempResult.Add(allStrings.FindByShortName("senha", false));			// Portuguese
tempResult.Add(allStrings.FindByShortName("sifre", false));			// Turkish
tempResult.Add(allStrings.FindByShortName("wachtwoord", false));	// Dutch
tempResult.Add(allStrings.FindByShortName("wagwoord", false));		// Afrikaans
tempResult.Add(allStrings.FindByShortName("watchword", false));		// English
tempResult.Add(allStrings.FindByShortName("zugangswort", false));	// German
*/
foreach (CxList r in tempResult)
{
	CSharpGraph g = r.data.GetByIndex(0) as CSharpGraph;
	if(g == null || g.ShortName == null)
	{
		continue;
	}
	if (g.ShortName.Length < 20)
	{
		result.Add(r);
	}
}