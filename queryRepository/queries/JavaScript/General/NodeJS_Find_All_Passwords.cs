CxList allStrings = All.FindByType("String");
allStrings.Add(Find_String_Literal());
allStrings.Add(Find_UnknownReference());
allStrings.Add(Find_Declarators());
allStrings.Add(Find_MemberAccesses());

CxList tempResult = allStrings.FindByShortNames(
	new List<string> {
		"*password*",
		"*psw",
		"psw*",
		"pwd*",
		"*pwd",
		"pass*"
		}, 
	false
	);

// Remove words from dictionary
CxList _toRemove1 = tempResult.FindByShortNames(
	new List<string> {
		"*pass",
		"*passable*",
		"*passenger*",
		"*passer*",
		"*passing*",
		"*passion*",
		"*passive*",
		"*passover*",
		"*passport*",
		"*passed*",
		"*compass*",
		"*bypass*"	
		}, 
	false
	);

tempResult -= _toRemove1;

tempResult.Add(allStrings.FindByShortNames(
	new List<string> {
		"adgangskode",			// Danish
		"benutzerkennwort",		// German
		"chiffre",				// German
		"clave", 				// Spanish
		"codewort",				// German
		"contrasena",			// Spanish
		"contrasenya",			// Catalan
		"geheimcode",			// German
		"geslo",				// Slovenian
		"heslo ",				// Czech
		"jelszo",				// Hungarian
		"kennwort",				// German
		"losenord",				// german
		"losung", 				// German
		"losungswort",			// German
		"lozinka",				// Bosnian, Croatian
		"modpas",				// Haitian-creole
		"motdepasse", 			// French
		"parol",				// Azerbaijani
		"parola", 				// Russian
		"parole",				// German
		"pasahitza", 			// Basque
		"pasfhocal", 			// Irish
		"passe",				// French
		"passord",				// Norwegian
		"passwort", 			// Spanish
		"pasvorto", 			// Esperanto
		"paswoord", 			// Dutch
		"salasana", 			// Finnish
		"schluessel",			// German
		"schluesselwort", 		// German
		"senha",				// Portuguese
		"sifre", 				// Turkish
		"wachtwoord",			// Dutch
		"wagwoord", 			// Afrikaans
		"watchword", 			// English
		"zugangswort", 			// German
		}, 
	false
	));


foreach (CxList r in tempResult)
{
	try{
		CSharpGraph g = r.GetFirstGraph();
		if(g == null || g.ShortName == null)
		{
			continue;
		}
		if (g.ShortName.Length < 20)
		{
			result.Add(r);
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}