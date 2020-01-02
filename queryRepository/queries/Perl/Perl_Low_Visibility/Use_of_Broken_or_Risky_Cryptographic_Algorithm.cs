/*
Do note that DES only uses 8 byte keys and only works on 8 byte data blocks.
If you're intending to encrypt larger blocks or entire files, please use Crypt::CBC in conjunction with this module.
See the Crypt::CBC documentation for proper syntax and use.

Also note that the DES algorithm is, by today's standard, weak encryption.
Crypt::Blowfish is highly recommended if you're interested in using strong encryption and a faster algorithm.
*/

CxList methods = Find_Methods();

result = methods.FindByShortName("Crypt::DES");


// Add LDAP that are not initialized to use HIGH ciphers
// (see: http://search.cpan.org/~marschap/perl-ldap-0.44/lib/Net/LDAP.pod)
// (see: http://www.nntp.perl.org/group/perl.ldap/2008/08/msg2922.html)

CxList newLDAP = All.FindByMemberAccess("Net::LDAP.new");
CxList HIGH = All.GetParameters(newLDAP).FindByShortName("HIGH");
CxList ciphers = All.GetParameters(newLDAP).FindByShortName("ciphers");
result.Add(newLDAP - newLDAP.FindByParameters(HIGH) * newLDAP.FindByParameters(ciphers));

// And the most basic - rand and srand
result.Add(methods.FindByShortName("rand") + methods.FindByShortName("srand"));


//MD5  SHA1 SHA MD2 MD4
/*

 use Digest::MD5;

 $ctx = Digest::MD5->new;

 $ctx->add($data);
 $ctx->addfile($file_handle);

 $digest = $ctx->digest;
 $digest = $ctx->hexdigest;
 $digest = $ctx->b64digest;

*/

CxList ma = All.FindByType(typeof(MemberAccess));
CxList digestFunctions = ma.FindByShortName("digest") +
	ma.FindByShortName("hexdigest") +
	ma.FindByShortName("b64digest") +
	ma.FindByShortName("transform");
CxList createNewMA = All.FindByMemberAccess("Digest::MD5.new") + 
	All.FindByMemberAccess("Digest::SHA1.new") +	
	All.FindByMemberAccess("Digest::HMAC_SHA1.new") +
	All.FindByMemberAccess("Digest::MD2.new") +
	All.FindByMemberAccess("Digest::Perl::MD4.new")+
	All.FindByMemberAccess("Digest::Perl::MD5.new") + 
	All.FindByMemberAccess("Digest::Perl::SHA1.new") +	
	All.FindByMemberAccess("Digest::Perl::HMAC_SHA1.new") +
	All.FindByMemberAccess("Digest::Perl::MD2.new") +
	All.FindByMemberAccess("Digest::MD4.new");
	

CxList parameters= All.GetParameters(All.FindByMemberAccess("Digest::SHA.new"));

createNewMA.Add(All.FindByParameters(parameters.FindByType(typeof(IntegerLiteral)).FindByShortName("1")));
CxList one = All.FindByShortName("1").FindByType(typeof(IntegerLiteral));
foreach(CxList p in parameters)
{
	if((p.DataInfluencedBy(one)).Count > 0)
	{
		createNewMA.Add(All.FindByParameters(p));
		
	}
}

CxList md5Ref = All.FindByType(typeof(UnknownReference)).FindByFathers(createNewMA.GetAncOfType(typeof(AssignExpr)));
md5Ref = md5Ref.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList allMD5Ref = All.FindByType(typeof(UnknownReference)).FindAllReferences(md5Ref);
result.Add(allMD5Ref.GetMembersOfTarget() * digestFunctions);
/*
use Digest::MD5 qw(md5 md5_hex md5_base64);
my $encrpass = md5($password);  # binary
    or
my $encrpass = md5_hex($password);  # human-readable
    or
my $encrpass = md5_base64($password);  # human-readable too
*/
result.Add(methods.FindByShortName("md5") +
	methods.FindByShortName("md5_hex") +
	methods.FindByShortName("md5_base64") +
	methods.FindByShortName("md2") +
	methods.FindByShortName("md2_base64") +
	methods.FindByShortName("md2_hex") +
	methods.FindByShortName("*sha1_hex") +
	methods.FindByShortName("*sha1") +
	methods.FindByShortName("*sha1_base64") +
	methods.FindByShortName("*sha1_trasnform") +
	methods.FindByShortName("md4") +
	methods.FindByShortName("md4_base64") +
	methods.FindByShortName("md4_hex"));