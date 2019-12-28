using System;

namespace CopyPictures
{
    internal interface IMediaCopier
    {
        void CopyMedia(string targetDir, string targetFile);
        DateTime GetDate();
    }
}
