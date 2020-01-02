string[] sObjects = {
	"Account", "AccountContactRole", "AccountFeed", "AccountHistory", "AccountOwnerSharingRule", "AccountPartner", "AccountShare", "AccountTag", "AccountTeamMember", "AccountTerritoryAssignmentRule", "AccountTerritoryAssignmentRuleItem", "AccountTerritorySharingRule", "ActivityHistory", "AdditionalNumber", "ApexClass", "ApexComponent", "ApexPage", "ApexTrigger", "Approval", "Asset", "AssetFeed", "AssetTag", "AssignmentRule", "Attachment",
	"Bookmark", "BrandTemplate", "BusinessHours", "BusinessProcess",
	"CallCenter", "Campaign", "CampaignFeed", "CampaignMember", "CampaignMemberStatus", "CampaignOwnerSharingRule", "CampaignShare", "CampaignTag", "Case", "CaseComment", "CaseContactRole", "CaseFeed", "CaseHistory", "CaseMilestone", "CaseOwnerSharingRule", "CaseShare", "CaseSolution", "CaseStatus", "CaseTag", "CaseTeamMember", "CaseTeamRole", "CaseTeamTemplate", "CaseTeamTemplateMember", "CaseTeamTemplateRecord", "CategoryData", "CategoryNode", "CategoryNodeLocalization", "Community", "Contact", "ContactFeed", "ContactHistory", "ContactOwnerSharingRule", "ContactShare", "ContactTag", "ContentDocument", "ContentDocumentHistory", "ContentVersion", "ContentVersionHistory", "ContentWorkspace", "ContentWorkspaceDoc", "Contract", "ContractContactRole", "ContractFeed", "ContractHistory", "ContractLineItem", "ContractLineItemHistory", "ContractStatus", "ContractTag", "CronTrigger", "CurrencyType",
	"DatedConversionRate", "Division", "DivisionLocalization", "Document", "DocumentAttachmentMap", "DocumentTag",
	"EmailMessage", "EmailServicesAddress", "EmailServicesFunction", "EmailStatus", "EmailTemplate", "Entitlement", "EntitlementContact", "EntitlementHistory", "EntityHistory", "EntitlementTemplate", "EntitySubscription", "Event",
	"FeedComment", "FeedTrackedChange", "FeedPost", "FiscalYearSettings", "Folder", "ForecastShare",
	"Group", "GroupMember",
	"Holiday",
	"Idea", "IdeaComment",
	"Lead", "LeadFeed", "LeadHistory", "LeadOwnerSharingRule", "LeadShare", "LeadStatus", "LeadTag", "LineitemOverride",
	"MailmergeTemplate", "MilestoneType",
	"Name", "NewsFeed", "Note", "NoteTag", "NoteAndAttachment",
	"OpenActivity", "Opportunity", "OpportunityCompetitor", "OpportunityContactRole", "OpportunityFeed", "OpportunityFieldHistory", "OpportunityHistory", "OpportunityLineItem", "OpportunityLineItemSchedule", "OpportunityOverride", "OpportunityOwnerSharingRule", "OpportunityPartner", "OpportunityShare", "OpportunityStage", "OpportunityTag", "OpportunityTeamMember", "Organization", "OrgWideEmailAddress",
	"Partner", "PartnerNetworkConnection", "PartnerNetworkRecordConnection", "PartnerRole", "Period", "Pricebook2", "PricebookEntry", "ProcessInstance", "ProcessInstanceHistory", "ProcessInstanceStep", "ProcessInstanceWorkitem", "Product2", "Product2Feed", "ProductEntitlementTemplate", "Profile",
	"QuantityForecast", "QuantityForecastHistory", "Question", "QueueSobject",
	"RecordType", "RecordTypeLocalization", "Reply", "RevenueForecast", "RevenueForecastHistory",
	"sobject",
	"Scontrol", "ScontrolLocalization", "SelfServiceUser", "ServiceContract", "ServiceContractHistory", "ServiceContractOwnerSharingRule", "ServiceContractShare", /*"Site",*/ "SiteHistory", "Solution", "SolutionFeed", "SolutionHistory", "SolutionStatus", "SolutionTag", "StaticResource",
	"TagDefinition", "Task", "TaskPriority", "TaskStatus", "TaskTag", "Territory",
	"User", "UserAccountTeamMember", "UserFeed", "UserLicense", "UserPreference", "UserProfileFeed", "UserRole", "UserTeamMember", "UserTerritory",
	"Vote",
	"WebLink", 
	"WebLinkLocalization"};

CxList nonTarget = All - All.GetTargetOfMembers().GetMembersOfTarget();

result.Add(nonTarget.FindByTypes(sObjects));

result.Add(Find_VF_Pages().FindAllReferences(result.GetFathers().FindByType(typeof(MethodDecl))));

result.Add(All.FindByShortName("*__sobject"));

result -= result.FindByName("$*");