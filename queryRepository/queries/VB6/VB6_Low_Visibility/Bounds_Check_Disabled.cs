CxList vbp = All.FindByFileName("*.vbp");
CxList boundsCheck = vbp.FindByShortName("BoundsCheck", false);
CxList zero = vbp.FindByShortName("0");

result = zero.GetByAncs(boundsCheck.GetFathers()).Concatenate(boundsCheck);