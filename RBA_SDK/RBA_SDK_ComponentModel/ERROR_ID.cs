namespace RBA_SDK_ComponentModel
{
    public enum ERROR_ID
    {
        RESULT_ERROR_JVM = -250, // 0xFFFFFF06
        RESULT_ERROR_FILE_OPERATION = -201, // 0xFFFFFF37
        RESULT_ERROR_MEMORY = -200, // 0xFFFFFF38
        RESULT_ERROR_ENCRYPTION_FAILED = -155, // 0xFFFFFF65
        RESULT_ERROR_PARAMETER_LEN = -154, // 0xFFFFFF66
        RESULT_ERROR_TERMINAL_LOCKED = -153, // 0xFFFFFF67
        RESULT_ERROR_PARAMETER = -152, // 0xFFFFFF68
        RESULT_ERROR_BUFFER_SIZE = -151, // 0xFFFFFF69
        RESULT_ERROR_INVALID_RESPONSE = -150, // 0xFFFFFF6A
        RESULT_ERROR_ALREADY_CONNECTED = -104, // 0xFFFFFF98
        RESULT_ERROR_NOT_CONNECTED = -103, // 0xFFFFFF99
        RESULT_ERROR_NOT_PAIRED = -102, // 0xFFFFFF9A
        RESULT_ERROR_TIMEOUT = -101, // 0xFFFFFF9B
        RESULT_ERROR_IO_COMMUNICATION = -100, // 0xFFFFFF9C
        RESULT_ERROR_NOT_INITIALIZED = -50, // 0xFFFFFFCE
        RESULT_ERROR_BUSY = -4, // 0xFFFFFFFC
        RESULT_ERROR_UNSUPPORTED = -3, // 0xFFFFFFFD
        RESULT_ERROR_NOT_AVAILABLE = -2, // 0xFFFFFFFE
        RESULT_ERROR = -1, // 0xFFFFFFFF
        RESULT_SUCCESS = 0
    }
}