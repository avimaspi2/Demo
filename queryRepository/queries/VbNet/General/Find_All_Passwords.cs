CxList tempResult = All.FindByShortName("password*", false);
tempResult.Add(All.FindByShortName("*password", false));
tempResult.Add(All.FindByShortName("*psw", false));
tempResult.Add(All.FindByShortName("psw*", false));
tempResult.Add(All.FindByShortName("pwd*", false));
tempResult.Add(All.FindByShortName("*pwd", false));
tempResult.Add(All.FindByShortName("pass*", false));

// Remove words from dictionary
CxList toRemove = tempResult.FindByShortName("*pass", false);
toRemove.Add(tempResult.FindByShortName("*passable*", false));
toRemove.Add(tempResult.FindByShortName("*passage*", false));
toRemove.Add(tempResult.FindByShortName("*passenger*", false));
toRemove.Add(tempResult.FindByShortName("*passer*", false));
toRemove.Add(tempResult.FindByShortName("*passing*", false));
toRemove.Add(tempResult.FindByShortName("*passion*", false));
toRemove.Add(tempResult.FindByShortName("*passive*", false));
toRemove.Add(tempResult.FindByShortName("*passover*", false));
toRemove.Add(tempResult.FindByShortName("*passport*", false));
toRemove.Add(tempResult.FindByShortName("*passed*", false));
toRemove.Add(tempResult.FindByShortName("*compass*", false));
toRemove.Add(tempResult.FindByShortName("*bypass*", false));
tempResult -= toRemove;

// Add passwords in other languages
List<String> passResult = new List<String>(new string[] {"pass",			
	"adgangskode", 				// Danish
	"benutzerkennwort", 		// German
	"chiffre", 					// German
	"clave", 					// Spanish
	"codewort", 				// German
	"contrasena", 				// Spanish
	"contrasenya", 				// Catalan
	"geheimcode", 				// German
	"geslo", 					// Slovenian
	"heslo ", 					// Czech
	"jelszo", 					// Hungarian
	"kennwort", 				// German
	"losenord", 				// Swedish
	"losung", 					// German
	"losungswort", 				// German
	"lozinka", 					// Bosnian, Croatian
	"modpas", 					// Haitian-creole
	"motdepasse", 				// French
	"parol", 					// Azerbaijani / Russian
	"parola", 					// Russian
	"parole", 					// German
	"pasahitza", 				// Basque
	"pasfhocal", 				// Irish
	"passe", 					// French
	"passord", 					// Norwegian
	"passwort", 				// Spanish
	"pasvorto", 				// Esperanto
	"paswoord", 				// Dutch
	"salasana", 				// Finnish
	"schluessel", 				// German
	"schluesselwort", 			// German
	"senha", 					// Portuguese
	"sifre", 					// Turkish
	"wachtwoord", 				// Dutch
	"wagwoord", 				// Afrikaans
	"watchword", 				// English
	"zugangswort", 				// German
	"PAROLACHIAVE", 			// Italian
	"PAROLA CHIAVE", 			// Italian
	"PAROLECHIAVI", 			// Italian
	"PAROLE CHIAVI", 			// Italian
	"paroladordine", 			// Spanish
	"verschluesselt", 			// German
	"sisma" 	});				// Hebrew

CxList passByLang = All.FindByShortNames(passResult,false);
tempResult.Add(passByLang);

foreach (CxList r in tempResult)
{
	try{
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
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}