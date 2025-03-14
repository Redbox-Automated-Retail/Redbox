using System.Collections.Generic;
using Redbox.NetCore.Middleware.Http;
using UpdateClientService.API.Services.FileSets;

namespace UpdateClientService.API.Services.IoT.FileSets
{
    public class ReportFileSetVersionsResponse : ApiBaseResponse
    {
        public List<ClientFileSetRevisionChangeSet> ClientFileSetRevisionChangeSets { get; set; }
    }
}