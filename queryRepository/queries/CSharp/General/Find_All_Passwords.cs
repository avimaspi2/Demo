CxList allRelevantTypes = All.FindByType(typeof(StringLiteral));
allRelevantTypes.Add(All.FindByType(typeof(UnknownReference)));
allRelevantTypes.Add(All.FindByType(typeof(Declarator)));
allRelevantTypes.Add(All.FindByType(typeof(MemberAccess)));
allRelevantTypes.Add(All.FindByType(typeof(PropertyDecl)));

List < string > tempResultStrings = new List<string> {
		"password*", "*password",
		"*psw", "psw*",
		"pwd*", "*pwd",
		"pass*"};

CxList tempResult = allRelevantTypes.FindByShortNames(tempResultStrings, false);
//remove types that won't contain passwords
String[] toRemoveTypesStrings = new String[] {
		"int", "Int16", "Int32", "Int64", "UInt64", "UInt32", "UInt16", "bool", "float", "double", "date", 
	"datetime", "decimal", "short", "long", "ulong", "uint", "single", "guid", "System.Int16", "System.Int32", 
	"System.Int64", "System.UInt64", "System.UInt32", "System.UInt16", "System.Boolean", "System.Double", "System.Date", 
	"System.Datetime", "System.Decimal", "System.Single"
		};
	
CxList toRemoveTypes = tempResult.FindByTypes(toRemoveTypesStrings, false);
tempResult -= toRemoveTypes;

// Remove words from dictionary
tempResultStrings = new List<string> {
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
		"*Attempt*"};

tempResult -= tempResult.FindByShortNames(tempResultStrings, false);

// Remove not passwords
tempResultStrings = new List<string> {
		"passwordAnswer",
		"passwordQuestion",
		"passwordLabel",
		"passwordFormat",
		"passwordAnswerFromDB",
		"passwordSalt",
		"PasswordStrengthRegularExpression"};

tempResult -= tempResult.FindByShortNames(tempResultStrings, false);

tempResultStrings = new List<string> {
		"pass",
		"adgangskode", // Danish
		"benutzerkennwort", // German
		"chiffre", // German
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
		"pasfhocal", // Basque
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
		"sisma"}; // Hebrew
				
tempResult.Add(allRelevantTypes.FindByShortNames(tempResultStrings, false));

foreach (CxList r in tempResult)
{
	CSharpGraph g = r.data.GetByIndex(0) as CSharpGraph;
	if(g == null || g.ShortName == null)
	{
		continue;
	}
	if (g.ShortName.Length < 50)
	{
		result.Add(r);
	}
}
//remove constant passwords of type password_XXX
CxList allConstants = All.FindByType(typeof (ConstantDeclStmt));
CxList declPass = result.FindByType(typeof (Declarator));
CxList allConstantPass = declPass.FindByShortName(allConstants);
CxList constNotPass = allConstantPass.FindByShortNames(new List<string> {
		"password_*",
		"psw_*",
		"pwd_*",
		"pass_*"}, false);
result -= constNotPass;

result.Add(Find_Password_In_Connection_String());