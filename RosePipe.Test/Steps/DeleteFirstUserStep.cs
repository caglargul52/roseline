namespace RosePipe.Test.Steps;

public class DeleteFirstUserStep : StepBase<ExampleBag>
{
    protected override async Task<ExampleBag> ProcessAsync(ExampleBag input)
    {
        input.Users.RemoveAt(0);

        return StepError(new Exception("deneme"));
        
        var output = input with
        {
            Users = input.Users
        };

        return Next(output);
    }
}

