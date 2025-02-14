using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Redbox.REDS.Framework
{
    internal class ZippedResourceBundleStorage : IResourceBundleStorage, IDisposable
    {
        private ZipFile m_zipFile;

        public void Dispose()
        {
            Close();
        }

        public void Open(string path)
        {
            m_zipFile = new ZipFile(path)
            {
                UseZip64 = UseZip64.Off
            };
            BundlePath = path;
        }

        public void Close()
        {
            if (m_zipFile == null)
                return;
            m_zipFile.Close();
        }

        public void Create(string path, string trimPrefix)
        {
            m_zipFile = ZipFile.Create(path);
            ((ZipNameTransform)m_zipFile.NameTransform).TrimPrefix = trimPrefix;
            m_zipFile.UseZip64 = UseZip64.Off;
            BundlePath = path;
        }

        public void AddEntry(string path, string trimPrefix)
        {
            if (string.IsNullOrEmpty(trimPrefix))
            {
                m_zipFile.Add(path);
            }
            else
            {
                var trimPrefix1 = ((ZipNameTransform)m_zipFile.NameTransform).TrimPrefix;
                ((ZipNameTransform)m_zipFile.NameTransform).TrimPrefix = trimPrefix;
                m_zipFile.Add(path);
                ((ZipNameTransform)m_zipFile.NameTransform).TrimPrefix = trimPrefix1;
            }
        }

        public void BeginUpdate()
        {
            if (m_zipFile == null)
                return;
            m_zipFile.BeginUpdate();
        }

        public void CommitUpdate()
        {
            if (m_zipFile == null)
                return;
            m_zipFile.CommitUpdate();
        }

        public byte[] GetEntryContent(string path)
        {
            return GetEntryContent(GetEntry(path));
        }

        public byte[] GetEntryContent(IResourceBundleStorageEntry entry)
        {
            if (entry == null)
                return null;
            using (var binaryReader = new BinaryReader(m_zipFile.GetInputStream(entry.Index)))
            {
                return binaryReader.ReadBytes((int)entry.Size);
            }
        }

        public IResourceBundleStorageEntry GetEntry(string path)
        {
            var entry = m_zipFile.GetEntry(path);
            if (entry == null)
                return null;
            return new ResourceBundleStorageEntry
            {
                Name = entry.Name,
                Size = entry.Size,
                Index = entry.ZipFileIndex
            };
        }

        public ReadOnlyCollection<IResourceBundleStorageEntry> GetResourceManifest()
        {
            var bundleStorageEntryList = new List<IResourceBundleStorageEntry>();
            if (m_zipFile != null)
                foreach (ZipEntry zipEntry in m_zipFile)
                    bundleStorageEntryList.Add(new ResourceBundleStorageEntry
                    {
                        Name = zipEntry.Name,
                        Size = zipEntry.Size,
                        Index = zipEntry.ZipFileIndex
                    });
            return bundleStorageEntryList.AsReadOnly();
        }

        public bool SupportsSave => false;

        public string BundlePath { get; internal set; }
    }
}