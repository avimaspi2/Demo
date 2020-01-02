// ComponentArt
CxList res = All.FindByMemberAccess("NumberInput.Value", false); // numbers only
res.Add(All.FindByMemberAccess("Calendar.FormatDate", false));  // Date only
   // Infragistics
   // FarPoint
res.Add(All.FindByMemberAccess("FpBoolean.CheckState", false));  // Boolean
res.Add(All.FindByMemberAccess("FpCalendar.CurrentDate", false)); 
res.Add(All.FindByMemberAccess("FpClock.CurrentTime", false)); 
res.Add(All.FindByMemberAccess("FpCurrency.Text", false)); 
res.Add(All.FindByMemberAccess("FpCurrency.Value", false)); 
res.Add(All.FindByMemberAccess("FpDatetime.Text", false)); 
res.Add(All.FindByMemberAccess("FpDatetime.Value", false)); 
res.Add(All.FindByMemberAccess("FpDouble.Text", false)); 
res.Add(All.FindByMemberAccess("FpDouble.Value", false)); 
res.Add(All.FindByMemberAccess("FpInteger.Text", false)); 
res.Add(All.FindByMemberAccess("FpInteger.Value", false)); 
res.Add(All.FindByMemberAccess("FpPercent.Text", false)); 
res.Add(All.FindByMemberAccess("FpPercent.Value", false)); 
result = res;