namespace RosePipe.Test.Steps;

public class GetAllUserStep : StepBase<ExampleBag>
{
    protected override async Task<ExampleBag> ProcessAsync(ExampleBag input)
    {
        var users = GetFakeUsers();

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