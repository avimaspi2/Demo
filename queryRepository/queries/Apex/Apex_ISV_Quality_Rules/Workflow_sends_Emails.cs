//Find whether Outbound Emails are being sent via Workflow
CxList workflowCode = All.FindByFileName("*.workflow");
CxList alerts = workflowCode.FindByShortName("alerts");
result = alerts.FindByType(typeof(UnknownReference));