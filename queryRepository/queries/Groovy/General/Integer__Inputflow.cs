/* Find integer overflow and underflow based on the inputs only (and random). When calling the Integer_Oveflow
   and Integer_Underflow we use this query, and add to it also the result for Integer.MIN_VALUE or 
   Integer.MAX_VALUE (as needed).
*/

// All inputs + maxInt value + random values (with no parameters)
CxList inputs = Find_Inputs();
CxList random = All.FindByMemberAccess("Random.NextLong", false);
random.Add(Get_ESAPI().FindByMemberAccess("Randomizer.getRandomLong")); // ESAPI
random.Add(Get_ESAPI().FindByMemberAccess("Randomizer.getRandomFloat")); // ESAPI
CxList randomWithParams = random.FindByParameters(All.GetParameters(random, 0));
// Leave only random with n params, otherwise it is bounded by the parameter value
inputs.Add(random - randomWithParams);

result = Integer__Flow(inputs);