namespace RosePipe.Test.Steps;

public class GetAllUserStep : StepBase<ExampleBag, CustomError>
{
    protected override async Task<ExampleBag> ProcessAsync(ExampleBag input)
    {
        var users = GetFakeUsers();

        if (!users.Any())
        {
            return ThrowStepError(new CustomError { Message = "Users is not found!", Code = "A1", Severity = 1 });
        }

        var output = input with
        {
            Users = users
        };

        return Next(output);
    }

    private List<string> GetFakeUsers()
    {
        return new List<string>
        {
            "Çağlar",
            "Osman",
            "Ismail"
        };
    }
}