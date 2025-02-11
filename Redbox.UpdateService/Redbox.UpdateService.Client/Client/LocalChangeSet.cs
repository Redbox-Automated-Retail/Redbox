using System.Collections.Generic;
using System.IO;
using Redbox.Core;

namespace Redbox.UpdateService.Client
{
    public class LocalChangeSet
    {
        internal readonly Dictionary<string, Dictionary<string, string>> m_newFiles =
            new Dictionary<string, Dictionary<string, string>>();

        internal readonly Dictionary<long, Dictionary<string, string>> m_updates =
            new Dictionary<long, Dictionary<string, string>>();

        public bool HashChanges()
        {
            return m_updates.Count > 0 || m_newFiles.Count > 0;
        }

        public bool AddUpdateToFile(long id, string localPath, string displayName)
        {
            if (!File.Exists(localPath))
                return false;
            if (!m_updates.ContainsKey(id))
                m_updates.Add(id, new Dictionary<string, string>());
            using (var fileStream = File.OpenRead(localPath))
            {
                m_updates[id]["Hash"] = fileStream.ToASCIISHA1Hash();
            }

            m_updates[id]["Data"] = localPath;
            m_updates[id]["DisplayName"] = displayName;
            return true;
        }

        public void UpdateDisplayName(long id, string displayName)
        {
            UpdateKey(id, "DisplayName", displayName);
        }

        public bool AddNewFile(
            string targetPath,
            string targetName,
            string displayName,
            string localPath)
        {
            if (!File.Exists(localPath))
                return false;
            var dictionary = new Dictionary<string, string>
            {
                {
                    "Name",
                    targetName
                },
                {
                    "Path",
                    targetPath
                },
                {
                    "DisplayName",
                    displayName
                },
                {
                    "Data",
                    localPath
                }
            };
            using (var fileStream = File.OpenRead(localPath))
            {
                dictionary["Hash"] = fileStream.ToASCIISHA1Hash();
            }

            m_newFiles.Add(Path.Combine(targetPath, targetName), dictionary);
            return true;
        }

        public void UndoChange(long id, string name)
        {
            if (!m_updates.ContainsKey(id) || !m_updates[id].ContainsKey(name))
                return;
            m_updates[id].Remove(name);
        }

        public void UndoAddFile(string fullPath)
        {
            if (!m_newFiles.ContainsKey(fullPath))
                return;
            m_newFiles.Remove(fullPath);
        }

        private void UpdateKey(long id, string name, string value)
        {
            if (!m_updates.ContainsKey(id))
            {
                m_updates.Add(id, new Dictionary<string, string>());
                m_updates[id].Add(name, value);
            }
            else if (m_updates[id].ContainsKey(name))
            {
                m_updates[id][name] = value;
            }
            else
            {
                m_updates[id].Add(name, value);
            }
        }
    }
}