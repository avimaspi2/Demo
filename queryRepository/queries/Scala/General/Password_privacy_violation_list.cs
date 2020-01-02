CxList allStrings = All.FindByType("String"); 
allStrings.Add(All.FindByType(typeof(StringLiteral))); 
allStrings.Add(Find_UnknownReference());
allStrings.Add(All.FindByType(typeof (Declarator)));
allStrings.Add(All.FindByType(typeof (MemberAccess)));
allStrings.Add(All.FindByType(typeof(EnumMemberDecl))); 
allStrings.Add(Find_Methods().FindByShortName("get*"));  // For getPassword() methods

List < String > tempResultStrings = new List<String> {
		"*password*",
		"*psw",
		"psw*",
		"pwd*",
		"*pwd",
		"*authKey*",
		"pass*"};
CxList tempResult = allStrings.FindByShortNames(tempResultStrings, false);		

// Remove words from dictionary
List < String > toRemoveStrings = new List<String> {
		"*pass",
		"*passable*",
		"*passage*",
		"*passenger*",
		"*passer*", 
		"*passing*",
		"*passion*",
		"*passive*",
		"*passover*",
		"*passport*",
		"*passed*", 
		"*compass*",
		"*bypass*",
		"pass-through",
		"passthru",
		"passthrough",
		"passbytes",
		"passcount",
		"passratio"};
CxList toRemove = tempResult.FindByShortNames(toRemoveStrings, false);
tempResult -= toRemove;

List < String > passwordStrings = new List<String> {
		"pass",
		"adgangskode",	// Danish
		"benutzerkennwort",// German
		"chiffre", 		// German
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
		"sisma" 		// Hebrew
		}; 

tempResult.Add(allStrings.FindByShortNames(passwordStrings, false));


foreach (CxList r in tempResult)
{
	CSharpGraph g = r.data.GetByIndex(0) as CSharpGraph;
	if(g != null && g.ShortName != null && g.ShortName.Length < 50)
	{
		result.Add(r);
	}
}