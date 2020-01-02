/*	Poor_Database_Access_Control
	
	This query looks for database queries relying
	on user-supplied primary keys. This is raised
	as an issue since it maybe the case that an
	attacker is able to fetch private data by
	arbitrarily changing the input. E.g. an
	online catalog whose products are displayed
	according to an ID coming from the end-user.
*/


CxList query = NodeJS_Find_DB_Base();
CxList queryParam = All.GetParameters(query);
CxList variables = Find_UnknownReference();
CxList memberAccesses = Find_MemberAccesses();
CxList input = NodeJS_Find_Interactive_Inputs() + NodeJS_Find_Read();

List <string> relevantNames = new List<string> {
	"_id", "id_", "id",
	"account", "accountid", "account_id", "_account",
	"product", "productid", "product_id",
	"user", "user_id", "user_account"
	};
CxList varsAndMemberAccesses = All.NewCxList();
varsAndMemberAccesses.Add(variables);
varsAndMemberAccesses.Add(memberAccesses);
CxList externalSources = varsAndMemberAccesses.FindByShortNames(relevantNames, false).InfluencedBy(input);
result = queryParam.InfluencedBy(externalSources).InfluencedBy(input);