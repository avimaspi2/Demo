/*
This query implements CWE 326: "Inadequate Encryption Strength"

Description Summary:
    "The software stores or transmits sensitive data using an encryption scheme that is 
     theoretically sound, but is not strong enough for the level of protection required".
*/

CxList passwords = Find_Passwords_Unsafe();
CxList personal_info = Find_Personal_Info();
CxList sensitiveData = All.NewCxList();
sensitiveData.Add(passwords);
sensitiveData.Add(personal_info);

// WeakEncrypt contains the calls to weak encrypter constructor/factory
CxList WeakEncrypt = Find_Weak_Hashes();
WeakEncrypt.Add(Find_Weak_Digest_Utils());

// Tracing the assignment/initialization to assigned variable, e.g md1/md2 in this example:
//   md2 = MessageDigest.getInstance("MD5");
//   MessageDigest md1 = MessageDigest.getInstance("MD5");   
CxList variableAssigned = All.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(WeakEncrypt.GetFathers());

// Add the encryptors too, for cases where they are called statically, e.g:
//    byte[] dgst = DigestUtils.md5(password);
// And their parameters are now also part of the "bad" sink.
variableAssigned.Add(WeakEncrypt);

// Find all references of the "tainted" object variable (i.e. md1/md2 throughout the code)
CxList WeakEncrypterReferences = All.FindAllReferences(variableAssigned);

// Add the calls which use the weak encrypter as a parameter allocated statically, e.g.:
//     MessageDigest dgst = DigestUtils.uptate(MessageDigest.getSha1Digest(), password);
WeakEncrypterReferences.Add(All.FindByParameters(WeakEncrypterReferences.GetTargetOfMembers()));

// Add the calls which use a previously-created weak encrypter as a parameter, e.g.:
//     MessageDigest weakencryptor= MessageDigest.getSha1Digest();
//     MessageDigest sink = DigestUtils.uptate(weakencryptor, password);
WeakEncrypterReferences.Add(All.FindByParameters(WeakEncrypterReferences));


// All parameters used in calls to methods of the object or class
WeakEncrypterReferences.Add(WeakEncrypterReferences.GetMembersOfTarget());
CxList WeakEncrypterParams = All.GetParameters(WeakEncrypterReferences);

result = sensitiveData.DataInfluencingOn(WeakEncrypterParams).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);