CxList inputs = Find_Inputs();

// Remove specific members of window.location and document.location from the inputs
CxList windowLocation = inputs.FindByMemberAccess("location", "*");
inputs -= windowLocation.FindByShortNames(new List<string>{"protocol","host","pathname","reload"});

result = inputs;