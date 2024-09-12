namespace RoseLine.Test.Steps;

public class DeleteFirstUserStep : StepBase<ExampleBag, CustomError>
{
    protected override async Task<ExampleBag> ProcessAsync(ExampleBag input)
    {
        input.Users.RemoveAt(0);
        
        var output = input with
        {
            Users = input.Users
        };

        return Next(output);
    }
}

