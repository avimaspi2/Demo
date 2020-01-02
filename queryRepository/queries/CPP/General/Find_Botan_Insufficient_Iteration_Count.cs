// According to the description, we assume 10.000 as the minimum secure threshold
// so we should yield a result for a value below 10.000

//Botan derive functions
CxList pbdkf2 = Find_Botan_PBDKF2_Function("pbdkf");
CxList deriveKey = Find_Botan_PBDKF2_Function("derive_key");
CxList pbdkfIterations = Find_Botan_PBDKF2_Function("pbdkf_iterations");
CxList pbdkfTimed = Find_Botan_PBDKF2_Function("pbdkf_timed");

//pbdkf(out[],out_len, passphrase, salt[], salt_len, iterations, msec)
result.Add(Find_Not_In_Range(pbdkf2, 5, 10000, null));

//Case 1: function has 4 parameters
//derive_key(out_len, passphrase, salt[], iterations)
CxList case1 = Filter_By_Parameter_Count(deriveKey, 4);
result.Add(Find_Not_In_Range(case1, 3, 10000, null));

//Case2: function has 5 parameters
//derive_key(out_len, passphrase, salt[], salt_len, iterations)
//derive_key(out_len, passphrase, salt[], msec, iterations)
//pbdkf_iterations(out_len, passphrase, salt[], salt_len, iterations)
CxList case2 = Filter_By_Parameter_Count(deriveKey + pbdkfIterations, 5);
result.Add(Find_Not_In_Range(case2, 4, 10000, null));


//Case 3: function has 6 parameters
//derive_key(out_len, passphrase, salt[], salt_len, msec, iterations)
//pbdkf_iterations(out[],out_len, passphrase, salt[], salt_len, iterations)
//pbdkf_timed(out_len, passphrase, salt[], salt_len, msec, iterations)
CxList case3 = Filter_By_Parameter_Count(deriveKey + pbdkfTimed + pbdkfIterations, 6);
result.Add(Find_Not_In_Range(case3, 5, 10000, null));


//Case 4: function has 7 parameters
//pbdkf_timed(out[],out_len, passphrase, salt[], salt_len, msec, iterations)
CxList case4 = Filter_By_Parameter_Count(pbdkfTimed, 7);
result.Add(Find_Not_In_Range(case4, 6, 10000, null));