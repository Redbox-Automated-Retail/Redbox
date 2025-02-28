using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UpdateClientService.API.Services.IoT.Commands
{
    public class IoTCommandModel
    {
        [JsonProperty("command")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandEnum Command { get; set; }

        [JsonProperty("messageType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageTypeEnum MessageType { get; set; }

        [JsonProperty("version")] public int Version { get; set; } = 1;

        [JsonProperty("requestId")] public string RequestId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("origin")] public string AppName { get; set; } = "UpdateServiceClient";

        [JsonProperty("payload")] public object Payload { get; set; }

        [JsonProperty("logRequest")] public bool? LogRequest { get; set; }

        [JsonProperty("logResponse")] public bool? LogResponse { get; set; }

        [JsonProperty("sourceId")] public string SourceId { get; set; }

        [JsonProperty("requestTimeUtc")] public DateTime RequestTimeUtc => DateTime.UtcNow;

        [JsonIgnore] public string ReturnServerId { get; set; }

        [JsonIgnore]
        public QualityOfServiceLevel QualityOfServiceLevel { get; set; } = QualityOfServiceLevel.AtLeastOnce;

        [JsonIgnore]
        public bool LogPayload
        {
            get
            {
                var logPayload = true;
                if (MessageType == MessageTypeEnum.Request)
                    logPayload = LogRequest ?? true;
                else if (MessageType == MessageTypeEnum.Response)
                    logPayload = LogResponse ?? true;
                return logPayload;
            }
        }
    }
}