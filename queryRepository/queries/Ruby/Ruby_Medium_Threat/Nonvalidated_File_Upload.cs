CxList attachedFile = Find_Methods().FindByShortName("has_attached_file");
CxList validation = Find_Methods().FindByShortName("validates_attachment_content_type");

CxList clsAttachedFile = All.GetClass(attachedFile);
CxList clsValidation = All.GetClass(validation);

CxList potentialClass = clsAttachedFile - clsValidation;

result = attachedFile.GetByAncs(potentialClass);