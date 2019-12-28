using System;
using System.IO;

namespace CopyPictures
{
    internal abstract class MediaCopier : IMediaCopier
    {
        #region Fields, Properties
        private FileInfo _FInfo;
        public FileInfo FInfo
        {
            get { return _FInfo; }
        }
        #endregion

        #region Construction
        public MediaCopier(FileInfo fInfo)
        {
            _FInfo = fInfo;
        }
        #endregion

        #region IMediaCopier
        public void CopyMedia(string targetDir, string targetFile)
        {
            if (!Directory.Exists(targetDir.ToString()))
            {
                Directory.CreateDirectory(targetDir.ToString());
            }

            if (!File.Exists(targetFile))
            {
                FInfo.CopyTo(targetFile, true);
                Console.WriteLine(String.Format("Kopiere {0}",FInfo.FullName));
            }
        }

        public abstract DateTime GetDate();
        #endregion
    }
}
