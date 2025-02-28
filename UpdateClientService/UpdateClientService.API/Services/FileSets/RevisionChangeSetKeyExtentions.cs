namespace UpdateClientService.API.Services.FileSets
{
    public static class RevisionChangeSetKeyExtentions
    {
        public static string IdentifyingText(this RevisionChangeSetKey revisionChangeSetKey)
        {
            return string.Format("FileSetId: {0}, FileSetRevisionId: {1}, PatchFileSetRevisionId: {2}",
                revisionChangeSetKey.FileSetId, revisionChangeSetKey.RevisionId, revisionChangeSetKey.PatchRevisionId);
        }
    }
}