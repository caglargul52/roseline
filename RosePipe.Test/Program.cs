using RosePipe.Test.Steps;

namespace RosePipe.Test;

class Program
{
    static async Task Main(string[] args)
    {
        var bag = new ExampleBag();

        var pipeline = new Pipeline<ExampleBag>(bag);

        pipeline.AddStep(new GetAllUserStep());
        pipeline.AddStep(new DeleteFirstUserStep());

        var pipelineResult = await pipeline.ExecuteAsync();

        Console.Read();
    }
}