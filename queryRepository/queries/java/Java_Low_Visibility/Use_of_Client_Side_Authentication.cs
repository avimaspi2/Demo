CxList htmlOutput = Find_Web_Outputs();
htmlOutput.Add(Find_Html_Outputs());

CxList strings = Find_Strings();
CxList htmlRemarks = strings.FindByName("*<script*");
htmlRemarks.Add(strings.FindByName("*</script>*"));

htmlOutput = htmlOutput.DataInfluencedBy(htmlRemarks);

CxList pass = Find_Password_Strings();

result = htmlOutput.DataInfluencedBy(pass);