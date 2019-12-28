using System;
using System.IO;

namespace CopyPictures
{
    internal class MovieCopier : MediaCopier
    {
        public MovieCopier(FileInfo fInfo) : base(fInfo)
        {
        }

        public override DateTime GetDate()
        {
            return FInfo.LastWriteTime;
        }
    }
}
