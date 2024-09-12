using RoseLine.Test.Steps;

namespace RoseLine.Test;

class Program
{
    static async Task Main(string[] args)
    {
        //initial bag definiton

        var bag = new ExampleBag
        {
            Users = []
        };

        var pipeline = new Pipeline<ExampleBag, CustomError>(bag);

        pipeline.AddStep(new GetAllUserStep());
        pipeline.AddStep(new DeleteFirstUserStep());

        var pipelineResult = await pipeline.ExecuteAsync();

        if (pipelineResult.IsSuccess)
        {
            Console.WriteLine(string.Join(",", pipelineResult.Bag.Users));
        }
        else
        {
            Console.WriteLine($"Step: {pipelineResult.Error.Step}, IsUnHandledError: {pipelineResult.Error.IsUnhandledError}, Code: {pipelineResult.Error.Code}");
        }

        Console.Read();
    }
}