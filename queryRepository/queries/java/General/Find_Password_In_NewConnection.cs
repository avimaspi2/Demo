CxList methods = Find_Methods();

CxList getConnection = methods.FindByMemberAccess("DriverManager.getConnection");
CxList dsGetConnection = methods.FindByMemberAccess("DataSource.getConnection");
CxList setProperty = methods.FindByMemberAccess("Properties.setProperty");
CxList passParam = Find_Strings().GetParameters(setProperty, 0).FindByShortName("password");

CxList password = All.GetParameters(getConnection, 2);

password.Add(All.GetParameters(dsGetConnection, 1));
password -= password.FindByType(typeof(Param));
if (passParam.Count > 0) {
	foreach(CxList temp in getConnection) {
		if (All.GetParameters(temp,2).Count == 0) {
			CxList propParam = All.GetParameters(temp, 1);
			CxList isconnection = propParam.DataInfluencedBy(passParam);
			if (isconnection.Count > 0) {
				password.Add(isconnection);
			}
		}
	}
}
result = password;