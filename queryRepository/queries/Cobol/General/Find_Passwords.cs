CxList allStrings = Find_Strings();
allStrings.Add(Find_Unknown_References());
allStrings.Add(Find_Declarators());
allStrings.Add(Find_MemberAccesses());

CxList tempResult = 
	allStrings.FindByShortNames(new List<string>{"*password*","*psw","psw*","*pwd","pwd*","pass*"}, false);

// Remove words from dictionary
tempResult -= tempResult.FindByShortNames(new List<string>{"*pass","*passable*","*passage*","*passenger*","*passer*",
		"*passing*","*passion*","*passive*","*passover*","*passport*","*passed*","*compass*","*bypass*"}, false);

tempResult.Add(allStrings.FindByShortName("userPass", false));

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
// Add passwords in varions languages
tempResult.Add(allStrings.FindByShortNames(new List<string>(tempToAddNames), false));

foreach (CxList r in tempResult)
{
	CSharpGraph g = r.TryGetCSharpGraph<CSharpGraph>();
	if(g == null || g.ShortName == null)
	{
		continue;
	}
	if (g.ShortName.Length < 20)
	{
		result.Add(r);
	}
}

result -= result.FindByType("bool");