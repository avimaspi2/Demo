CxList cryptoImports = All.NewCxList();

cryptoImports.Add(All.FindByMemberAccess("\"golang.org/x/crypto/md4\".New"));
cryptoImports.Add(All.FindByMemberAccess("\"crypto/md5\".New"));
cryptoImports.Add(All.FindByMemberAccess("\"crypto/sha1\".New"));
cryptoImports.Add(All.FindByMemberAccess("\"crypto/sha256\".*").FindByShortNames(new List<string>{"New", "New224"}));

List<string> newMethods = new List<string>{"New", "New384", "New512_224", "New512_256"};

cryptoImports.Add(All.FindByMemberAccess("\"crypto/sha512\".*").FindByShortNames(newMethods));
cryptoImports.Add(All.FindByMemberAccess("\"golang.org/x/crypto/ripemd160\".New"));

List<string> shaMethods = new List<string>{"New224", "New256", "New384", "New512"};

cryptoImports.Add(All.FindByMemberAccess("\"golang.org/x/crypto/sha3\".*").FindByShortNames(shaMethods));
cryptoImports.Add(All.FindByMemberAccess("\"crypto/hmac\".New"));

result = cryptoImports;