using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JustGimme
{
    public class HardFile
    {
        private string filePath;

        private List<byte> bytes;
        public List<byte> Bytes
        {
            get 
            {
                if (bytes != null)
                {
                    return bytes;
                }
                else
                {
                    return new List<byte>(File.ReadAllBytes(filePath));
                }
            }
        }

        private bool exists;
        public bool Exists
        {
            get { return exists; }
        }

        private string shortName;
        public string ShortName
        {
            get { return shortName; }
        }

        private FileInfo fileinfo;
        public FileInfo Information
        {
            get { return fileinfo; }
        }

        public int PacketAmounts
        {
            get
            {
                return (int)(fileinfo.Length / 8192);
            }
        }

        private FileStream stream;
        public FileStream Stream
        {
            get
            {
                return stream;
            }
        }

        private long bytesStreamed;
        private long bytesTotal;
        public bool NeedToStream
        {
            get
            {
                return (bytesStreamed < bytesTotal);
            }
        }

        public HardFile(string fp)
        {
            filePath = fp;
            exists = File.Exists(filePath);
            if (exists)
            {
                fileinfo = new FileInfo(filePath);
                stream = new FileStream(filePath, FileMode.Open);
                bytesStreamed = 0;
                bytesTotal = fileinfo.Length;
            }
            shortName = filePath.Split('\\')[filePath.Split('\\').Count() - 1];
        }

        public void LoadToMemory()
        {
            bytes = new List<byte>(File.ReadAllBytes(filePath));
        }

        public byte[] StreamBytes()
        {
            if (this.exists)
            {
                byte[] buff = new byte[8192];
                stream.Read(buff, 0, 8192);
                bytesStreamed += 8192;
                return buff;
            }
            else
            {
                return null;
            }
        }
    }
}
