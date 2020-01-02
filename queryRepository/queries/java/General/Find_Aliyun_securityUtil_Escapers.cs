string[] aliyunEscapers = new string[]{"SecurityUtil.jsEncode", "SecurityUtil.escapeHtml"};
result = Find_Methods().FindByMemberAccesses(aliyunEscapers);