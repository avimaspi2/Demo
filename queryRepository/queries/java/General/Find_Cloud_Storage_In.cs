/**
*	This query will gather storage inputs from cloud services
*/

result.Add(All.FindByMemberAccess("AmazonS3.getObject"));
result.Add(All.FindByMemberAccess("S3Object.getObject"));
result.Add(All.FindByMemberAccess("AmazonS3.generatePresignedUrl"));
result.Add(All.FindByMemberAccess("AmazonS3.copyObject"));
result.Add(All.FindByMemberAccess("AmazonS3.listObjects"));
result.Add(All.FindByMemberAccess("AmazonS3Client.getObjectAsString"));
result.Add(All.FindByMemberAccess("AmazonS3.getObjectAsString"));
result.Add(All.FindByMemberAccess("S3Object.getObjectMetadata"));
result.Add(All.FindByMemberAccess("GetObjectRequest.getObjectContent"));

//Instances of Amazon's amazonaws.services.s3.transfer.TransferManager:
result.Add(All.FindByMemberAccess("TransferManager.download"));
result.Add(All.FindByMemberAccess("TransferManager.downloadDirectory"));