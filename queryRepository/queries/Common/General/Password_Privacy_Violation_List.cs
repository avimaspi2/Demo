CxList allStrings = All.FindByType("String"); 
allStrings.Add(Find_Strings()); 
allStrings.Add(Find_UnknownReference());
allStrings.Add(Find_Declarators());
allStrings.Add(Find_MemberAccesses());
allStrings.Add(Find_EnumMemberDecl()); 
allStrings.Add(Find_Methods().FindByShortName("get*"));  // For getPassword() methods

List<string> pswdIncludeList = new List<string>{"*password*", "*psw", "psw*", "pwd*", "*pwd", "*authKey*", "pass*", "cipher*", "*cipher", "pass", "adgangskode", "benutzerkennwort", "chiffre", "clave", "codewort", "contrasena", "contrasenya", "geheimcode", "geslo", "heslo", "jelszo", "kennwort", "losenord", "losung", "losungswort", "lozinka", "modpas", "motdepasse", "parol", "parola", "parole", "pasahitza", "pasfhocal", "passe", "passord", "passwort", "pasvorto", "paswoord", "salasana", "schluessel", "schluesselwort", "senha", "sifre", "wachtwoord", "wagwoord", "watchword", "zugangswort", "PAROLACHIAVE", "PAROLA CHIAVE", "PAROLECHIAVI", "PAROLE CHIAVI", "paroladordine", "verschluesselt", "sisma"};
List<string> pswdExcludeList = new List<string>{"*pass", "*passable*", "*passage*", "*passenger*", "*passer*", "*passing*", "*passion*", "*passive*", "*passover*", "*passport*", "*passed*", "*compass*", "*bypass*", "pass-through", "passthru", "passthrough", "passbytes", "passcount", "passratio"};

CxList tempResult = allStrings.FindByShortNames(pswdIncludeList, false);
CxList toRemove = tempResult.FindByShortNames(pswdExcludeList, false);
tempResult -= toRemove;
tempResult.Add(allStrings.FindByShortName("pass", false));

foreach (CxList r in tempResult)
{
	CSharpGraph g = r.TryGetCSharpGraph<CSharpGraph>();
	if(g != null && g.ShortName != null && g.ShortName.Length < 50)
	{
		result.Add(r);
	}
}