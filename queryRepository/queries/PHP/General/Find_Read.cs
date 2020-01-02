CxList methods = Find_Methods();
result = Find_File_Read();
	
CxList socket = methods.FindByName("*socket_*");
CxList stream = methods.FindByName("*stream_*");
CxList socketInput = 
	socket.FindByName("*socket_read") + 
	socket.FindByName("*socket_recv") + 
	socket.FindByName("*socket_recvfrom*") + 
	socket.FindByName("*socket_import_stream") +
	socket.FindByName("*socket_get_line");
	
CxList streamInput = 
	stream.FindByName("*stream_socket_recvfrom") +
	stream.FindByName("*stream_get_line") +
	stream.FindByName("*stream_get_meta_data") +
	stream.FindByName("*stream_get_contents");
	
CxList streamWrapperObjects = All.FindByType("*StreamWrapper*");	
CxList streamWrapperInputMethods = streamWrapperObjects.FindByMemberAccess("stream_read*");

result.Add(socketInput + streamInput + streamWrapperInputMethods);

result.Add(Find_Smarty_Read());

//memcache: add get methods influenced by the query
result.Add(Find_memcache_Inputs(result));