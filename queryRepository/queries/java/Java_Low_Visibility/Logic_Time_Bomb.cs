CxList methods = Find_Methods();
CxList getInstance = methods.FindByShortName("getInstance");

CxList exec = methods.FindByMemberAccess("Runtime.exec"); 
exec.Add(methods.FindByMemberAccess("getRuntime.exec"));
exec.Add(methods.FindByMemberAccess("System.exec"));
exec.Add(methods.FindByMemberAccess("Executor.safeExec"));


CxList conditions = Find_Conditions();

CxList time_cond = All.FindByMemberAccess("Calendar.after");
time_cond.Add(All.FindByMemberAccess("Duration.isZero"));
time_cond.Add(All.FindByMemberAccess("Duration.isNegative"));
time_cond.Add(All.FindByMemberAccess("Instant.isAfter"));
time_cond.Add(All.FindByMemberAccess("Instant.isZero"));
time_cond.Add(All.FindByMemberAccess("LocalDate.isAfter"));
time_cond.Add(All.FindByMemberAccess("LocalDate.isBefore"));
time_cond.Add(All.FindByMemberAccess("LocalDate.isEqual"));
time_cond.Add(All.FindByMemberAccess("LocalDate.isLeapYear"));
time_cond.Add(All.FindByMemberAccess("LocalDateTime.isAfter"));
time_cond.Add(All.FindByMemberAccess("LocalDateTime.isBefore"));
time_cond.Add(All.FindByMemberAccess("LocalDateTime.isEqual"));
time_cond.Add(All.FindByMemberAccess("LocalTime.isAfter"));
time_cond.Add(All.FindByMemberAccess("LocalTime.isBefore"));
time_cond.Add(All.FindByMemberAccess("MonthDay.isAfter"));
time_cond.Add(All.FindByMemberAccess("MonthDay.isBefore"));
time_cond.Add(All.FindByMemberAccess("MonthDay.isValidYear"));
time_cond.Add(All.FindByMemberAccess("LocalDate.isLeapYear"));
time_cond.Add(All.FindByMemberAccess("OffsetDateTime.isAfter"));
time_cond.Add(All.FindByMemberAccess("OffsetDateTime.isBefore"));
time_cond.Add(All.FindByMemberAccess("OffsetDateTime.isEqual"));
time_cond.Add(All.FindByMemberAccess("OffsetTime.isAfter"));
time_cond.Add(All.FindByMemberAccess("OffsetTime.isBefore"));
time_cond.Add(All.FindByMemberAccess("OffsetTime.isEqual"));
time_cond.Add(All.FindByMemberAccess("Period.isZero"));
time_cond.Add(All.FindByMemberAccess("Year.isAfter"));
time_cond.Add(All.FindByMemberAccess("Year.isBefore"));
time_cond.Add(All.FindByMemberAccess("Year.isLeap"));
time_cond.Add(All.FindByMemberAccess("YearMonth.isAfter"));
time_cond.Add(All.FindByMemberAccess("YearMonth.isBefore"));
time_cond.Add(All.FindByMemberAccess("YearMonth.isLeapYear"));


time_cond = time_cond.DataInfluencedBy(getInstance);

CxList afterInCondition = conditions.DataInfluencedBy(time_cond) + conditions * time_cond;

result = exec.GetByAncs(afterInCondition.GetFathers());