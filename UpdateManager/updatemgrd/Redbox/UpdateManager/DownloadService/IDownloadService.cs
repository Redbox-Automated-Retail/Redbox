using Redbox.UpdateManager.ComponentModel;
using System.Collections.Generic;

namespace Redbox.UpdateManager.DownloadService
{
    internal interface IDownloadService
    {
        List<IDownloader> GetDownloads();

        ErrorList AddDownload(
          string key,
          string hash,
          string url,
          DownloadPriority priority,
          out IDownloader download);
    }
}
