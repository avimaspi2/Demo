result = Find_Methods().FindByShortName("session_start");

//handle session gereation using memcache
//code sample example:
/*
$seq_session_id = 0 ;
		if($memcache_obj = memcache_pconnect('localhost')){
			$seq_session_id = memcache_get($memcache_obj,'seq_session_id') ;
			$seq_session_id+= 1 ;
			memcache_set($memcache_obj,'seq_session_id',$seq_session_id) ;
		}
*/

CxList memCache = All.FindByShortName("memcache_set").FindByType(typeof(MethodInvokeExpr));
memCache.Add(All.FindByMemberAccess("Memcache.set"));
result.Add(memCache.FindByParameters(All.FindByShortName("seq_session_id")));