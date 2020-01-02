CxList tempResult = All.NewCxList();

// Get all strings to be analised
CxList allStrings = All.FindByType("String");
allStrings.Add(All.FindByType("byte"));
allStrings.Add(Find_Declarators());
allStrings.Add(Find_MemberAccesses());
allStrings.Add(Find_UnknownReference());
allStrings.Add(All.FindByType(typeof(PropertyDecl)));
allStrings.Add(Find_Strings());

// Get only the strings that have sugestive names
List<string> includeNames = new List<string>{"*password", "*psw", "psw*", "pwd*", "*pwd", "*authKey*", "pass*", "cipher*", "*cipher", "pass", "adgangskode", "benutzerkennwort", "chiffre", "clave", "codewort", "contrasena", "contrasenya", "geheimcode", "geslo", "heslo", "jelszo", "kennwort", "losenord", "losung", "losungswort", "lozinka", "modpas", "motdepasse", "parol", "parola", "parole", "pasahitza", "pasfhocal", "passe", "passord", "passwort", "pasvorto", "paswoord", "salasana", "schluessel", "schluesselwort", "senha", "sifre", "wachtwoord", "wagwoord", "watchword", "zugangswort", "PAROLACHIAVE", "PAROLA CHIAVE", "PAROLECHIAVI", "PAROLE CHIAVI", "paroladordine", "verschluesselt", "sisma"};
tempResult.Add(allStrings.FindByShortNames(new List<string>(includeNames), false));


// Remove the strings with the following names
List<string> excludeNames = new List<string>{"*pass", "*passable*", "*passage*", "*passenger*", "*passer*", "*passing*", "*passion*", "*passive*", "*passover*", "*passport*", "*passed*", "*compass*", "*bypass*", "pass-through", "passthru", "passthrough", "passbytes", "passcount", "passratio"};
CxList toRemoveResult = allStrings.FindByShortNames(new List<string>(excludeNames), false);
tempResult -= toRemoveResult;


result = tempResult;