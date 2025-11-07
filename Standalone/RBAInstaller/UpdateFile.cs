namespace RBAInstaller
{
    public class UpdateFile
    {
        public string Path { get; set; }

        public string CompareVersion { get; set; }

        public string MinorVersion { get; set; }

        public bool IsIntermediateVasKey { get; set; }
    }
}