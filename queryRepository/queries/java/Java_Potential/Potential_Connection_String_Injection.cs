CxList con = All.FindByName("*getConnection");

CxList inputs = Find_Potential_Inputs();

result = con.DataInfluencedBy(inputs);