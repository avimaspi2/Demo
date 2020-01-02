CxList methods = Find_Methods();
List < String > bad_random_methods = new List<String>(){"rand", "mt_rand", "srand", "mt_srand", "str_shuffle", "shuffle", "array_rand"};
result = methods.FindByShortNames(bad_random_methods);