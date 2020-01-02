// According to the description, we assume 10.000 as the minimum secure threshold
// so we should yield a result for a value below 10.000
result = Find_Not_In_Range("golang.org/x/crypto/pbkdf2",  "Key", 2, 10000, null);