CxList allStrings = All.FindByType("String");
allStrings.Add(Find_Strings());
allStrings.Add(Find_UnknownReference());
allStrings.Add(Find_Declarators());
allStrings.Add(Find_MemberAccesses()); 

List<string> allStringsNames = new List<string>{
		"*password","password*","*psw","psw*","pw*","pwd*","*pwd",
		"pass*", "*secret_key", "secret_key*", "secretkey*","*secretkey", 
		// also support string variants
		"*password'","'password*","*psw'","'psw*","'pw*","'pwd*","*pwd'",
		"'pass*", "\"password*","\"password*","*psw\"","\"psw*","\"pw*",
		"\"pwd*","*pwd\"","\"pass*", "\"secret_key\"", "\"secret_key*", 
		"*secret_key\"", "\'secret_key\'", "\'secret_key*", "*secret_key\'", 
		"\"secretkey\"", "\"secretkey*", "*secretkey\"", "\'secretkey\'", 
		"\'secretkey*", "*secretkey\'"
		};

CxList tempResult = allStrings.FindByShortNames(allStringsNames, false);

// Remove words from dictionary
List<string> toRemoveResultNames = new List<string>{	
		// Remove words from dictionary
		"*pass","*passable*", "*passage*", "*passenger*", 
		"*passer*", "*passing*", "*passion*", "*passive*", 
		"*passover*", "*passport*", "*passed*", "*compass*", "*bypass*"	
		};
CxList toRemoveResult = tempResult.FindByShortNames(toRemoveResultNames, false);

tempResult -= toRemoveResult;

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

// remove password holders with more than 20 chars
foreach (CxList r in tempResult)
{
	CSharpGraph g = r.GetFirstGraph();
	if (g != null && g.ShortName != null && g.ShortName.Length < 20){
		result.Add(r);
	}
}

//remove constant passwords of type password_XXX
CxList constPass = result.FindByType(typeof(ConstantDeclStmt));
CxList declPass = result.FindByType(typeof(Declarator));

CxList allConstantPass = declPass.FindByShortName(constPass);
allConstantPass.Add(constPass);

List<string> constNotPassNames = new List<string>{	
		"password_*","psw_*","pwd_*","pass_*"
		};
CxList constNotPass = allConstantPass.FindByShortNames(constNotPassNames, false); 

result -= constNotPass;