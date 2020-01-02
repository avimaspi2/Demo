CxList cipher = All.FindByMemberAccess("Cipher.getInstance");
CxList strings = Find_Strings();
CxList RSAString = strings.FindByShortName("*RSA*");
CxList noPadding = strings.FindByShortName("*NoPadding*");

result = cipher.DataInfluencedBy(RSAString).DataInfluencedBy(noPadding);