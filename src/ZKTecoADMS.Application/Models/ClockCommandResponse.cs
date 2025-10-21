using System.Text.Json.Serialization;
using ZKTecoADMS.Application.Constants;

namespace ZKTecoADMS.Application.Models;

public class ClockCommandResponse
{

    public long CommandId { get; set; }
    public int Return { get; set; }
    public string CMD { get; set; }
    
    [JsonIgnore]
    public ClockCommandResponses Response { get; set; }
    
    [JsonIgnore]
    public string Message => Response.GetDescription();
    
    [JsonIgnore]
    public bool IsSuccess => Response.IsSuccess();
}