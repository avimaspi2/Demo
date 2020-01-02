CxList inputs = All.FindByMemberAccess("request.userhostname") + 
				All.FindByMemberAccess("request.filter") + 
				All.FindByMemberAccess("request.userhostaddress") + 
				All.FindByMemberAccess("request.filepath") + 
				All.FindByMemberAccess("request.contentencoding") + 
				All.FindByMemberAccess("request.contenttype") + 
				All.FindByMemberAccess("request.accepttypes") + 
				All.FindByMemberAccess("request.applicationpath") + 
				All.FindByMemberAccess("request.apprelativecurrentexecutionfilepath") + 
				All.FindByMemberAccess("request.currentexecutionfilepath") + 
				All.FindByMemberAccess("request.gethashcode") + 
				All.FindByMemberAccess("request.gettype") + 
				All.FindByMemberAccess("request.httpmethod") + 
				All.FindByMemberAccess("request.isauthenticated") + 	
				All.FindByMemberAccess("request.islocal") + 
				All.FindByMemberAccess("request.issecureconnection") + 
				All.FindByMemberAccess("request.logonuseridentity") + 
				All.FindByMemberAccess("request.mappath") + 
				All.FindByMemberAccess("request.physicalapplicationpath") + 
				All.FindByMemberAccess("request.requesttype") + 
				All.FindByMemberAccess("request.servervariables") + 
				All.FindByMemberAccess("request.validateinput");
result = inputs + inputs.GetTargetOfMembers();