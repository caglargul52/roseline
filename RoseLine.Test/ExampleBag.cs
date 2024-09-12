namespace RoseLine.Test;

public sealed record ExampleBag : BagBase
{
    public List<string> Users { get; set; } = [];
}