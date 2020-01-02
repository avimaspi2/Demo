CxList classes = All.FindByType(typeof(ClassDecl));
 
//Ruby Test Unit Classes
result = classes.InheritsFrom("Unit.TestCase");
result.Add(classes.InheritsFrom("ActiveRecord.TestCase"));
result.Add(classes.InheritsFrom("ActionController.TestCase"));
result.Add(classes.InheritsFrom("ActionView.TestCase"));
result.Add(classes.InheritsFrom("ActiveSupport.TestCase"));
result.Add(classes.InheritsFrom("ActionMailer.TestCase"));
result.Add(classes.InheritsFrom("ActionDispatch.IntegrationTest"));
result.Add(classes.InheritsFrom("ActionController.IntegrationTest"));

result.Add(classes.FindByRegex(@"(\s)?Test::Unit::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?MiniTest::Unit::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?ActiveRecord::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?ActionController::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?ActionView::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?ActiveSupport::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?ActionMailer::TestCase"));
result.Add(classes.FindByRegex(@"(\s)?ActionDispatch::IntegrationTest"));
result.Add(classes.FindByRegex(@"(\s)?ActionController::IntegrationTest"));