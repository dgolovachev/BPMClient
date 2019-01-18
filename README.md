## C# Service and Process starter for BPM`Online.

## Usage example

## Start process:
```cs
var config = new BpmClientConfig("https:/bpmonline", "LOGIN", "PASSWORD");
var client = new BpmClient(config);

var param = new Dictionary<string, object>
{
    {"param1", "some_value"},
    {"param2", DateTime.Now},
    {"param3",3 }
};
client.StartProcess("SOME_PROCESS", param);
```

## Start process with result:
```cs
var config = new BpmClientConfig("https:/bpmonline", "LOGIN", "PASSWORD");
var client = new BpmClient(config);

var param = new Dictionary<string, object>
{
    {"param1", "some_value"},
    {"param2", DateTime.Now},
    {"param3",3 }
};
var result = client.StartProcess("SOME_PROCESS", param, "result_parameter_name");
Console.WriteLine(result);
```

## Call service:
```cs
var param = new Dictionary<string, object>
{
    {"param1", "some_value"},
    {"param2", DateTime.Now},
    {"param3",3 }
};
var result = client.CallSeviceGet("SOME_SERVICE", "METHOD", param);
Console.WriteLine(result);
```
