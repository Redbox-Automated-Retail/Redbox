using System.Collections.Generic;


namespace RBAInstaller
{
    public class UpdateJson
    {
        public string Revision { get; set; }

        public List<UpdateFile> Files { get; set; }
    }
}