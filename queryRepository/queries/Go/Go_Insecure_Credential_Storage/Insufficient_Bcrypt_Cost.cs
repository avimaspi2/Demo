CxList withCostValuesOutOfRange = All.NewCxList();
CxList goodConstants = All.NewCxList();
CxList vars = All.NewCxList();

withCostValuesOutOfRange.Add(Find_Not_In_Range("golang.org/x/crypto/bcrypt", "GenerateFromPassword", 1, 10, null));

List<string> bcryptMethods = new List<string>{"MaxCost","DefaultCost"};

CxList bycryptMembers = All.FindByMemberAccess("\"golang.org/x/crypto/bcrypt\".*").FindByShortNames(bcryptMethods);

goodConstants.Add(bycryptMembers);

withCostValuesOutOfRange -= goodConstants;

vars.Add(bycryptMembers.GetAssignee());

CxList references = All.FindAllReferences(vars);

withCostValuesOutOfRange -= references;
result.Add(withCostValuesOutOfRange);