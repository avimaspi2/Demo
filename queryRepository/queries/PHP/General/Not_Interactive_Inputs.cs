CxList inputs = All.FindByMemberAccess("Request.UserHostName") + 
				All.FindByMemberAccess("Request.Filter") + 
				All.FindByMemberAccess("Request.UserHostAddress") + 
				All.FindByMemberAccess("Request.FilePath") + 
				All.FindByMemberAccess("Request.ContentEncoding") + 
				All.FindByMemberAccess("Request.ContentType") + 
				All.FindByMemberAccess("Request.AcceptTypes") + 
				All.FindByMemberAccess("Request.ApplicationPath") + 
				All.FindByMemberAccess("Request.AppRelativeCurrentExecutionFilePath") + 
				All.FindByMemberAccess("Request.CurrentExecutionFilePath") + 
				All.FindByMemberAccess("Request.GetHashCode") + 
				All.FindByMemberAccess("Request.GetType") + 
				All.FindByMemberAccess("Request.HttpMethod") + 
				All.FindByMemberAccess("Request.IsAuthenticated") + 	
				All.FindByMemberAccess("Request.IsLocal") + 
				All.FindByMemberAccess("Request.IsSecureConnection") + 
				All.FindByMemberAccess("Request.LogonUserIdentity") + 
				All.FindByMemberAccess("Request.MapPath") + 
				All.FindByMemberAccess("Request.PhysicalApplicationPath") + 
				All.FindByMemberAccess("Request.RequestType") + 
				All.FindByMemberAccess("Request.ServerVariables") + 
				All.FindByMemberAccess("Request.ValidateInput") +

				All.FindByMemberAccess("Url.AbsolutePath") +
				All.FindByMemberAccess("Url.Authority") +
				All.FindByMemberAccess("Url.DnsSafeHost") +
				All.FindByMemberAccess("Url.Host") +
				All.FindByMemberAccess("Url.HostNameType") +
				All.FindByMemberAccess("Url.IsAbsoluteUri") +
				All.FindByMemberAccess("Url.IsDefaultPort") +
				All.FindByMemberAccess("Url.IsFile") +
				All.FindByMemberAccess("Url.IsLoopback") +
				All.FindByMemberAccess("Url.IsLoopback") +
				All.FindByMemberAccess("Url.IsUnc") +
				All.FindByMemberAccess("Url.LocalPath") +
				All.FindByMemberAccess("Url.Port") +
				All.FindByMemberAccess("Url.Scheme") +
				All.FindByMemberAccess("Url.UserEscaped");
result = inputs + inputs.GetTargetOfMembers();