CxList gradleObjects = Find_Gradle_Build_Objects();

CxList releaseMethods = Find_Gradle_Method("release");

CxList methodInvokesInRelease = gradleObjects.GetByAncs(releaseMethods).FindByType(typeof(MethodInvokeExpr));

CxList passwordsMethodInvoke = methodInvokesInRelease.FindByShortName("storePassword");
passwordsMethodInvoke.Add(methodInvokesInRelease.FindByShortName("keyPassword"));

result = gradleObjects.GetParameters(passwordsMethodInvoke, 0).FindByType(typeof(StringLiteral));