CxList releaseMethod = Find_Gradle_Method("release");
CxList gradleBuildObjects = Find_Gradle_Build_Objects();
CxList methodInvokesUnderRelease = gradleBuildObjects.FindByType(typeof(MethodInvokeExpr)).GetByAncs(releaseMethod);

CxList minifyEnabled = methodInvokesUnderRelease.FindByShortName("minifyEnabled");

CxList minifyValue = gradleBuildObjects.GetParameters(minifyEnabled, 0);
CxList minifyValueTrue = minifyValue.FindByShortName("true");

if (minifyValueTrue.Count == 0) {
	minifyValue = minifyValue.FindByAbstractValue(abstractValue => abstractValue is TrueAbstractValue);
} else {
	minifyValue = minifyValueTrue;	
}

if (minifyValue.Count == 0)
{
	CxList buildTypes = Find_Gradle_Method("buildTypes");
	if (buildTypes.Count > 0) {
		result = buildTypes;
	} else {
		result = Find_Gradle_Method("android");
	}
}