namespace UpdateClientService.API.Services.Transfer
{
    internal enum BG_ERROR_CONTEXT
    {
        BG_ERROR_CONTEXT_NONE,
        BG_ERROR_CONTEXT_UNKNOWN,
        BG_ERROR_CONTEXT_GENERAL_QUEUE_MANAGER,
        BG_ERROR_CONTEXT_QUEUE_MANAGER_NOTIFICATION,
        BG_ERROR_CONTEXT_LOCAL_FILE,
        BG_ERROR_CONTEXT_REMOTE_FILE,
        BG_ERROR_CONTEXT_GENERAL_TRANSPORT
    }
}