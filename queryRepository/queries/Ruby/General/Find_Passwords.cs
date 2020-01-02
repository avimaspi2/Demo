CxList allStrings = All;

CxList tempResult = 
	allStrings.FindByShortName("*password*", false) + 
	allStrings.FindByShortName("*psw", false) + 
	allStrings.FindByShortName("psw*", false) + 
	allStrings.FindByShortName("pwd*", false) + 
	allStrings.FindByShortName("*pwd", false) + 
	allStrings.FindByShortName("pass*", false);

// Remove words from dictionary
tempResult -= tempResult.FindByShortName("*pass", false);
tempResult -= tempResult.FindByShortName("*passable*", false);
tempResult -= tempResult.FindByShortName("*passage*", false);
tempResult -= tempResult.FindByShortName("*passenger*", false);
tempResult -= tempResult.FindByShortName("*passer*", false);
tempResult -= tempResult.FindByShortName("*passing*", false);
tempResult -= tempResult.FindByShortName("*passion*", false);
tempResult -= tempResult.FindByShortName("*passive*", false);
tempResult -= tempResult.FindByShortName("*passover*", false);
tempResult -= tempResult.FindByShortName("*passport*", false);
tempResult -= tempResult.FindByShortName("*passed*", false);

tempResult -= tempResult.FindByShortName("*compass*", false);
tempResult -= tempResult.FindByShortName("*bypass*", false);

// Add passwords in other languages
string[] tempToAddNames = 
	{"pass",
	"adgangskode",// Danish
	"benutzerkennwort",// German
	"chiffre", 	// German
	"clave", // Spanish
	"codewort", // German
	"contrasena", // Spanish
	"contrasenya", // Catalan
	"geheimcode", // German
	"geslo", // Slovenian
	"heslo ", // Czech
	"jelszo", // Hungarian
	"kennwort", // German
	"losenord", // Swedish
	"losung", // German
	"losungswort", // German
	"lozinka", // Bosnian, Croatian
	"modpas", // Haitian-creole
	"motdepasse", // French
	"parol", // Azerbaijani / Russian
	"parola", // Russian
	"parole", // German
	"pasahitza", // Basque
	"pasfhocal", // Irish / Basque
	"passe", // French
	"passord", // Norwegian
	"passwort", // Spanish
	"pasvorto", // Esperanto
	"paswoord", // Dutch
	"salasana", // Finnish
	"schluessel", // German
	"schluesselwort", // German
	"senha", // Portuguese
	"sifre", // Turkish
	"wachtwoord", // Dutch
	"wagwoord", // Afrikaans
	"watchword", // English
	"zugangswort", // German
	"PAROLACHIAVE", // Italian
	"PAROLA CHIAVE", // Italian
	"PAROLECHIAVI", // Italian
	"PAROLE CHIAVI", // Italian
	"paroladordine", // Spanish
	"verschluesselt", // German
	"sisma" // Hebrew
	};

tempResult.Add(allStrings.FindByShortNames(new List<string>(tempToAddNames), false));

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