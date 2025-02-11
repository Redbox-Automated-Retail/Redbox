using System;

namespace DeviceService.ComponentModel.Responses
{
    public class UnitDataModel
    {
        public string Manufacture { get; set; }

        public string DeviceType { get; set; }

        public string UnitSerialNumber { get; set; }

        public string RamSize { get; set; }

        public string FlashSize { get; set; }

        public string DigitizerVersion { get; set; }

        public string SecurityModuleVersion { get; set; }

        public string OSVersion { get; set; }

        public string ApplicationVersion { get; set; }

        public string EFTLVersion { get; set; }

        public string EFTPVersion { get; set; }

        public string ManufacturingSerialNumber { get; set; }

        public string DCKernelType { get; set; }

        public string EMVEngineKernelType { get; set; }

        public string CLessDiscoverKernelType { get; set; }

        public string ClessExpresPayV2KernelType { get; set; }

        public string ClessExpresPayV3KernelType { get; set; }

        public string ClessPayPassV3KernelType { get; set; }

        public string ClessPayPassV3AppType { get; set; }

        public string ClessVisaPayWaveKernelType { get; set; }

        public string ClessInteracKernelType { get; set; }

        public string ClessInterfaceIsSupported { get; set; }

        public bool IsTampered
        {
            get
            {
                var manufacturingSerialNumber = ManufacturingSerialNumber;
                return manufacturingSerialNumber != null &&
                       manufacturingSerialNumber.Equals("ALERT", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string GetApplicationVersionString()
        {
            try
            {
                var length = ApplicationVersion.Length;
                var minorVersion = ApplicationVersion[length - 1];
                return string.Format("{0}.{1}.{2}", ApplicationVersion.Substring(0, 2),
                    ApplicationVersion.Substring(2, length - 3), GetMinorVersionDecimal(minorVersion));
            }
            catch
            {
                return null;
            }
        }

        private static int GetMinorVersionDecimal(char minorVersion)
        {
            return char.IsDigit(minorVersion) ? minorVersion - 48 : minorVersion - 55;
        }
    }
}