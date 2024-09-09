namespace RosePipe.Test.Steps;

public class DeleteFirstUserStep : StepBase<ExampleBag>
{
    protected override async Task<ExampleBag> ProcessAsync(ExampleBag input)
    {
        input.Users.RemoveAt(0);

        return ThrowStepError(new CustomError()
        {
            Code = "12314",
            Message = "Merhaba",
        });
        
        var output = input with
        {
            Users = input.Users
        };

        return Next(output);
    }
}

