using Newtonsoft.Json;
using Samurai.Application.Saving;

namespace Samurai.Example;

public class ExampleModel : ISavable
{
    [JsonIgnore]
    public string Id => "Example";

    public string Data;

    public ExampleModel(string data) => Data = data;
}