namespace Redbox.Rental.Model.KioskClientService.Transactions
{
    public enum TransactionStatus
    {
        ADD_TO_CART = 1,
        AUTHORIZED = 2,
        SETTLEMENT_FAILED = 3,
        SETTLED = 4,
        DECLINED = 5,
        AVS_FAILED = 6,
        HARDWARE_ERROR = 7,
        DISK_NOT_TAKEN = 8,
        HARDWARE_CRITICAL_ERROR_1 = 9,
        HARD_ERROR_2 = 10, // 0x0000000A
        EMPTY_OR_STUCK = 11, // 0x0000000B
        EMPTY_SCAN = 12, // 0x0000000C
        SCAN_FAILED = 13, // 0x0000000D
        KIOSK_FULL = 14, // 0x0000000E
        CUSTOMER_NOT_ACTIVE = 15, // 0x0000000F
        RENTAL_LIMIT_EXCEEDED = 16, // 0x00000010
        PROMOTION_CODE_ALREADY_USED = 17, // 0x00000011
        RESERVED = 18, // 0x00000012
        FAILED = 19, // 0x00000013
        PROCESSOR_ERROR = 20, // 0x00000014
        RESERVATION_EXPIRED = 21, // 0x00000015
        CVV_MISMATCH = 22, // 0x00000016
        KIOSK_NOT_REGISTERED = 23, // 0x00000017
        INSUFFICIENT_KIOSK_INVENTORY = 24, // 0x00000018
        KIOSK_COMM_DOWN = 25, // 0x00000019
        KIOSK_RESERVATION_ATTEMPT_FAILED = 26, // 0x0000001A
        PENDING_AUTHORIZATION = 27, // 0x0000001B
        NOT_VENDED = 28, // 0x0000001C
        PRODUCT_LIMIT_ENFORCED = 29, // 0x0000001D
        GAME_LIMIT_EXCEEDED = 30, // 0x0000001E
        RECHECK_CREDIT_USE = 31, // 0x0000001F
        CREDIT_FAILURE = 32, // 0x00000020
        INSUFFICIENT_FUNDS = 33, // 0x00000021
        FRAUD_DECLINED = 34, // 0x00000022
        PURCHASE_RESERVATION_EXPIRED = 35 // 0x00000023
    }
}