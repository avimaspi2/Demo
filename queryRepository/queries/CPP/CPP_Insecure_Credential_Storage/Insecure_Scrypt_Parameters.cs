/*
	Based in the Colin Percival's paper: 
	https://www.tarsnap.com/scrypt/scrypt.pdf
	This query shows results when one of the parameters
	are in the following intervals:

	N < 32768 
	r < 8 
	p < 2
*/

CxList res = All.NewCxList();

CxList N = Find_Not_In_Range("sodium.h", "crypto_pwhash_scryptsalsa208sha256_ll", 4, 32768, null);
CxList r = Find_Not_In_Range("sodium.h", "crypto_pwhash_scryptsalsa208sha256_ll", 5, 8, null);
CxList p = Find_Not_In_Range("sodium.h", "crypto_pwhash_scryptsalsa208sha256_ll", 6, 2, null);

res.Add(N);
res.Add(r);
res.Add(p);

result.Add(res);