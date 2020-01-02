// VF files into a variable
CxList VF = Find_VF_Pages();

// Find setters for sObjects in a VF page
CxList sObjectSet = Find_sObjects().GetMembersOfTarget().FindByShortName("set*");

// Find problematic apex:inputText (which should have been inputField), and similar
CxList inputText = VF.FindByName("*.apex.inputtext.value")
	+ VF.FindByName("*.apex.inputtextarea.value")
	+ VF.FindByName("*.apex.inputhidden.value")
	+ VF.FindByName("*.apex.inputsecret.value");
 
// ...and BINGO!
result = inputText.InfluencedByAndNotSanitized(sObjectSet, Find_Test_Code());