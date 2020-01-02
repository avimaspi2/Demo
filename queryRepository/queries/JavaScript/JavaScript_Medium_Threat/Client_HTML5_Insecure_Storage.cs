//finds all sensitive data  saved into storage
//this is done by finding the flow from any indicator of sensitive data to the first parameter of stoarage setItem
CxList personalInfo = Find_Personal_Info();
CxList storageIn = Find_Storage_Inputs();
CxList irrelevant = Find_Parameters(); 
irrelevant.Add(Find_Binarys());
CxList valueToStore = (All - irrelevant).GetByAncs(All.GetParameters(storageIn));
CxList target = (valueToStore * personalInfo);
CxList sanitizer = Sanitize();

result = valueToStore.InfluencedByAndNotSanitized(personalInfo - target, sanitizer);
result.Add(target);