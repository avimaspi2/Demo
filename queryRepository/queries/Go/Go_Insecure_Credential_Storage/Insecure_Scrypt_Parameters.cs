/*
This query should yield results when the parameters are as follows 
( based on the default recommendations in the scrypt paper )
	N < 32768 
	r < 8 
	p > 2
*/

CxList N = Find_Not_In_Range("golang.org/x/crypto/scrypt", "Key", 2, 32768, null);
CxList r = Find_Not_In_Range("golang.org/x/crypto/scrypt", "Key", 3, 8, null);
CxList p = Find_Not_In_Range("golang.org/x/crypto/scrypt", "Key", 4, null, 2);

CxList res = N;
res.Add(r);
res.Add(p);

result.Add(res);