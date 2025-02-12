using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;

namespace LzmaAlone.Properties
{
    [ComVisible(true)]
    public class Settings : ApplicationSettingsBase
    {
        private static Settings m_Value;
        private static object m_SyncObject = new object();

        public static Settings Value
        {
            get
            {
                if (m_Value == null)
                {
                    Monitor.Enter(m_SyncObject);
                    if (m_Value == null)
                        try
                        {
                            m_Value = new Settings();
                        }
                        finally
                        {
                            Monitor.Exit(m_SyncObject);
                        }
                }

                return m_Value;
            }
        }
    }
}