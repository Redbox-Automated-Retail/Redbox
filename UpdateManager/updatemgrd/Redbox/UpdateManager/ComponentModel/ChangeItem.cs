using System.IO;

namespace Redbox.UpdateManager.ComponentModel
{
    internal class ChangeItem
    {
        public string VersionHash { get; set; }

        public string ContentHash { get; set; }

        public string TargetName { get; set; }

        public string TargetPath { get; set; }

        public bool IsSeed { get; set; }

        public bool Composite { get; set; }

        public string FormatFileName() => Path.Combine(this.TargetPath, this.TargetName);
    }
}
