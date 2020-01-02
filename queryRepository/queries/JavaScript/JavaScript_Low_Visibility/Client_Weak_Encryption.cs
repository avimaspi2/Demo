// Using DES algorithm is not secure
result.Add(Find_Members("CryptoJS.DES.encrypt").FindByType(typeof(MethodInvokeExpr)));

// Using crypto's generateCRMFRequest algorithm is not secure (https://developer.mozilla.org/en-US/docs/Archive/Mozilla/Typescript_crypto/generateCRMFRequest)
result.Add(Find_Members("crypto.generateCRMFRequest").FindByType(typeof(MethodInvokeExpr)));