// Query Non_Random_IV
// ===================
// (IV - Initilize Vector)
// Code example
/*
		byte[] text = "Secret".getBytes();
		byte[] ivBad = {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
		KeyGenerator kg = KeyGenerator.getInstance("DES");
		kg.init(56);
		SecretKey key = kg.generateKey();
		Cipher cipher = Cipher.getInstance("DES/ECB/PKCS5Padding");
		IvParameterSpec ipsBad = new IvParameterSpec(ivBad);
		cipher.init(Cipher.ENCRYPT_MODE, key, ipsBad);
		cipher.doFinal(text);
				
		byte[] ivGood = new byte[8];
		rand.nextBytes(ivGood);
		IvParameterSpec ipsGood = new IvParameterSpec(ivGood);
		cipher.init(Cipher.ENCRYPT_MODE, key, ipsGood);
		cipher.doFinal(text);
*/
// In code above ivBad should be recognized

CxList createExpr = All.FindByType(typeof(ObjectCreateExpr)).FindByName("*IvParameterSpec*");
CxList paramsExpr = All.GetParameters(createExpr, 0);
CxList paramsExprDecl = All.FindDefinition(All.FindAllReferences(paramsExpr));

CxList random = All.FindByType("*Random") + All.FindByMemberAccess("Math.Random");
CxList methodsOfRandom = All.FindAllReferences(random).GetMembersOfTarget();
CxList paramsRandom = All.GetParameters(methodsOfRandom, 0);
CxList paramsRandomDecl = All.FindDefinition(All.FindAllReferences(paramsRandom));
CxList nonRandomDecl = paramsExprDecl - paramsRandomDecl;
result = paramsExpr.FindAllReferences(nonRandomDecl);