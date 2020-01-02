if(param.Length > 0)
{
	string methodName = param[0] as string;
	if(!string.IsNullOrEmpty(methodName))
	{
		CxList gradleObjects = Find_Gradle_Build_Objects();
		CxList invokesOfMethodName = gradleObjects.FindByType(typeof(MethodInvokeExpr)).FindByShortName(methodName);
		CxList anonymousInvokeParam = gradleObjects.GetParameters(invokesOfMethodName, 0);
		result = gradleObjects.FindDefinition(anonymousInvokeParam);
	}
}