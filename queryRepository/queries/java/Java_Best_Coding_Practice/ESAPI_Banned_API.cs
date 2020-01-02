/*
The following APIs are banned when using ESAPI, because there are good ESAPI substitute for them:

 System.out.println() -> Logger.*
 Throwable.printStackTrace() -> Logger.*
 Runtime.exec() -> Executor.safeExec()
 Reader.readLine() -> Validator.safeReadLine()
 Session.getId() -> Randomizer.getRandomString() (better not to use at all)
 ServletRequest.getUserPrincipal() -> Authenticator.getCurrentUser()
 ServletRequest.isUserInRole() -> AccessController.isAuthorized*()
 Session.invalidate() -> Authenticator.logout()
 Math.Random.* -> Randomizer.*
 File.createTempFile() -> Randomizer.getRandomFilename()
 ServletResponse.setContentType() -> HTTPUtilities.setContentType()
 ServletResponse.sendRedirect() -> HTTPUtilities.sendSafeRedirect()
 RequestDispatcher.forward() -> HTTPUtilities.sendSafeForward()
 ServletResponse.addHeader() -> HTTPUtilities.addSafeHeader()
 ServletResponse.addCookie() -> HTTPUtilities.addSafeCookie()
 ServletRequest.isSecure() -> HTTPUtilties.isSecureChannel()
 Properties.* -> EncryptedProperties.*
 ServletContext.log() -> Logger.*
 java.security and javax.crypto -> Encryptor.*
 java.net.URLEncoder/Decoder -> Encoder.encodeForURL/decodeForURL
 java.sql.Statement.execute -> PreparedStatement.execute
 ServletResponse.encodeURL -> HTTPUtilities.safeEncodeURL (better not to use at all)
 ServletResponse.encodeRedirectURL -> HTTPUtilities.safeEncodeRedirectURL (better not to use at all)
*/

result = All.NewCxList();
CxList pomFile = All.FindByFileName("*\\pom.xml");
CxList allWithoutPomFile = All - pomFile - Find_Properties_Files();

CxList allOutPrints = allWithoutPomFile.FindByMemberAccess("out.print*");
CxList outPrints = Find_Methods().FindByName("out.print*");
CxList nonJspOutPrints = allOutPrints - outPrints.FindByFileName("*.jsp");
nonJspOutPrints -= outPrints.FindByFileName("*.vm");
result.Add(nonJspOutPrints);

result.Add(allWithoutPomFile.FindByMemberAccess("Throwable.printStackTrace"));
result.Add(allWithoutPomFile.FindByMemberAccess("Runtime.exec"));
result.Add(allWithoutPomFile.FindByMemberAccess("Reader.readLine"));
result.Add(allWithoutPomFile.FindByMemberAccess("Session.getId"));
result.Add(allWithoutPomFile.FindByMemberAccess("Session.invalidate"));
result.Add(allWithoutPomFile.FindByMemberAccess("Math.Random"));
result.Add(allWithoutPomFile.FindByMemberAccess("File.createTempFile"));
result.Add(allWithoutPomFile.FindByMemberAccess("RequestDispatcher.forward"));

result.Add(allWithoutPomFile.FindByMemberAccess("Properties.getProperty"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.list"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.load*"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.propertyNames"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.save"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.setProperty"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.store*"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.stringPropertyNames"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.clear"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.clone"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.contains"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.containsKey"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.containsValue"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.elements"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.entrySet"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.equals"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.get"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.hashCode"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.isEmpty"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.keys"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.keySet"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.put"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.putAll"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.rehash"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.remove"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.size"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.toString"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.values"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.finalize"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.getClass"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.notify"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.notifyAll"));
result.Add(allWithoutPomFile.FindByMemberAccess("Properties.wait"));

result.Add(allWithoutPomFile.FindByMemberAccess("java.security"));
result.Add(allWithoutPomFile.FindByMemberAccess("javax.crypto"));
result.Add(allWithoutPomFile.FindByMemberAccess("net.URLEncoder"));
result.Add(allWithoutPomFile.FindByMemberAccess("net.URLDecoder"));
result.Add(allWithoutPomFile.FindByMemberAccess("Statement.execute"));
result.Add(allWithoutPomFile.FindByMemberAccess("ServletContext.log"));
result.Add(allWithoutPomFile.FindByMemberAccess("ServletRequest.getUserPrincipal"));
result.Add(allWithoutPomFile.FindByMemberAccess("ServletRequest.isUserInRole"));
result.Add(allWithoutPomFile.FindByMemberAccess("ServletRequest.isSecure"));
CxList temp = allWithoutPomFile.FindByMemberAccess("ServletResponse.*");
result.Add(temp.FindByMemberAccess("ServletResponse.setContentType"));
result.Add(temp.FindByMemberAccess("ServletResponse.sendRedirect"));
result.Add(temp.FindByMemberAccess("ServletResponse.addHeader"));
result.Add(temp.FindByMemberAccess("ServletResponse.addCookie"));
result.Add(temp.FindByMemberAccess("ServletResponse.encodeURL"));
result.Add(temp.FindByMemberAccess("ServletResponse.encodeRedirectURL"));

// Remove false positives:
result -= result.FindByMemberAccess("EncryptedProperties.getProperty");
result -= result.FindByMemberAccess("EncryptedProperties.keySet");
result -= result.FindByMemberAccess("EncryptedProperties.load");
result -= result.FindByMemberAccess("EncryptedProperties.store");

result -= result.FindByMemberAccess("ReferenceEncryptedProperties.getProperty");
result -= result.FindByMemberAccess("ReferenceEncryptedProperties.keySet");
result -= result.FindByMemberAccess("ReferenceEncryptedProperties.load");
result -= result.FindByMemberAccess("ReferenceEncryptedProperties.store");

result -= result.FindByMemberAccess("DefaultEncryptedProperties.getProperty");
result -= result.FindByMemberAccess("DefaultEncryptedProperties.keySet");
result -= result.FindByMemberAccess("DefaultEncryptedProperties.load");
result -= result.FindByMemberAccess("DefaultEncryptedProperties.store");
result -= result.FindByMemberAccess("PreparedStatement.execute");