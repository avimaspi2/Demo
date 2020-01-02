//Find Gradle object only in build.gradle files
CxList gradleObjects = Find_Gradle_Objects();
result = gradleObjects.FindByFileName(@"*\build.gradle");