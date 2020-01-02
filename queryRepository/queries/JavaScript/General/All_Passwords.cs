CxList all = Find_String_Literal();

all.Add(Find_UnknownReference());
all.Add(Find_Declarators());
all.Add(Find_MemberAccesses());
all.Add(Find_ParamDecl());

//add the arrays 
all.Add(Find_FieldDecls());

CxList results = Find_String_Short_Name(all, new List<string>{
		"*password", "password*", "*psw", "psw*", "*pwd", 
		"pwd*", "pass*"}, false);

results -= Find_String_Short_Name(results, new List<string>{
		"*pass", "*passable*", "*passage*", "*passenger*", 
		"*passer*", "*passing*", "*passion*", "*passive*", 
		"*passover*", "*passport*", "*passed*",
		"passthrough*",
		"*compass", "*bypass"}, false);
		
results.Add(Find_String_Short_Name(all, new List<string>{
		"pass", 
		"adgangskode", 			// Danish
		"benutzerkennwort", 	// German
		"chiffre", 				// German
		"clave", 				// Spanish
		"codewort", 			// German
		"contrasena", 			// Spanish
		"contrasenya", 			// Catalan
		"geheimcode", 			// German
		"geslo", 				// Slovenian
		"heslo ", 				// Czech
		"jelszo", 				// Hungarian
		"kennwort", 			// German
		"losenord", 			// Swedish
		"losung", 				// German
		"losungswort", 			// German
		"lozinka", 				// Bosnian, Croatian
		"modpas", 				// Haitian-creole
		"motdepasse", 			// French
		"parol", 				// Azerbaijani / Russian
		"parola", 				// Russian
		"parole", 				// German
		"pasahitza", 			// Basque
		"pasfhocal", 			// Irish
		"passe", 				// French
		"passord", 				// Norwegian
		"passwort", 			// Spanish
		"pasvorto", 			// Esperanto
		"paswoord", 			// Dutch
		"salasana", 			// Finnish
		"schluessel", 			// German
		"schluesselwort", 		// German
		"senha", 				// Portuguese
		"sifre", 				// Turkish
		"wachtwoord", 			// Dutch
		"wagwoord", 			// Afrikaans
		"watchword", 			// English
		"zugangswort", 			// German
		"PAROLACHIAVE",			// Italian
		"PAROLA CHIAVE", 		// Italian
		"PAROLECHIAVI", 		// Italian
		"PAROLE CHIAVI", 		// Italian
		"paroladordine", 		// Spanish
		"verschluesselt", 		// German
		"sisma", 				// Hebrew
		}, false));

// CamelCase password
results.Add(Find_String_Short_Name(all, "*Pass", true));

foreach (CxList r in results){
	try{
		CSharpGraph g = r.GetFirstGraph();
		if(g != null && g.ShortName != null && g.ShortName.Length < 20){
			result.Add(r);
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}