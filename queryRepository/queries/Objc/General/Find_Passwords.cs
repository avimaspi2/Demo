//CxList all = All;
string[] tempResultNames = {"*password*", "*psw", "psw*", "pwd*", "*pwd", "pass*"};
CxList tempResult = All.FindByShortNames(new List<string>(tempResultNames), false);

// Remove words from dictionary
string[] tempToRemoveNames = {"*pass", "*passable*", "*passage*", "*passenger*", "*passer*", "*passing*", "*passion*",
	"*passive*", "*passover*", "*passport*", "*passed*", "*compass*", "*bypass*"};
tempResult -= tempResult.FindByShortNames(new List<string>(tempToRemoveNames), false);

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
	"heslo", // Czech
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
tempResult.Add(All.FindByShortNames(new List<string>(tempToAddNames), false));

CxList typesToRemove = All.NewCxList();	
// remove strings that are format strings
typesToRemove.Add(tempResult.FindByShortName("*%@*", false));
// remove namespaces, classes and methods that contain the word password
typesToRemove.Add(tempResult.FindByType(typeof(MethodDecl)));
typesToRemove.Add(tempResult.FindByType(typeof(ClassDecl)));
typesToRemove.Add(tempResult.FindByType(typeof(NamespaceDecl)));
typesToRemove.Add(tempResult.FindByType(typeof(InterfaceDecl)));

tempResult -= typesToRemove;

// Remove long sentences that make no sense as password
// remove NON string literals that are all capitals. This is most likely a constant and not a password
foreach (CxList p in tempResult)
{
	String name = p.GetName();
	if (name.Length < 50)
	{
		// If it's not a string literal make sure it's not capital
		if (!name.Equals(name.ToUpper()) || p.FindByType(typeof(StringLiteral)).Count > 0)
		{
			result.Add(p);
		}
	}
}
CxList methods = Find_Methods();
CxList ur = Find_UnknownReference();
CxList refAndMembers = Find_MemberAccess();
refAndMembers.Add(ur);

// Add SecAddSharedWebCredential third parameter
CxList SecAddSharedWebCredential = methods.FindByShortName("SecAddSharedWebCredential");
CxList SecAddSharedWebCredentialPass = All.GetParameters(SecAddSharedWebCredential, 2);
// Cases when converting NSString to CFString in the method invoke
CxList castExpr = SecAddSharedWebCredentialPass.FindByType(typeof(CastExpr));
SecAddSharedWebCredentialPass.Add(refAndMembers.FindByFathers(castExpr));

// Add the first parameter of LAContext setCredential:type:, if the type indicates a password
CxList LAContext = refAndMembers.FindByType("LAContext");
CxList LAContextMethods = LAContext.GetMembersOfTarget().FindByShortName("setCredential:type:");
CxList LACredentialType = All.GetParameters(LAContextMethods, 1);
CxList LACredentialTypePass = LACredentialType.FindByShortName("LACredentialTypeApplicationPassword");
LACredentialTypePass.Add(LACredentialType.FindByMemberAccess("LACredentialType.applicationPassword"));
CxList LAContextMethodPass = LAContextMethods.FindByParameters(LACredentialTypePass);
SecAddSharedWebCredentialPass.Add(All.GetParameters(LAContextMethodPass, 0));

// Seperate the results which are methods - as to not add their references
CxList SecAddSharedWebCredentialMethods = SecAddSharedWebCredentialPass * methods;
SecAddSharedWebCredentialPass -= SecAddSharedWebCredentialMethods;
result.Add(SecAddSharedWebCredentialMethods);

SecAddSharedWebCredentialPass -= Find_Null_Literals();
result.Add(All.FindAllReferences(SecAddSharedWebCredentialPass));