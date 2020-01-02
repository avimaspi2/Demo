//This query check whether there is an outbound email call
CxList emailSend = All.FindByMemberAccess("Messaging.sendEmail", false);
result = emailSend;