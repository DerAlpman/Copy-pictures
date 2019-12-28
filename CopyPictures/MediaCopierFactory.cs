using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CopyPictures
{
    internal static class MediaCopierFactory
    {
        internal static IMediaCopier CreateMediaCopier(string type, FileInfo fInfo)
        {
            IMediaCopier mediaCopier = null;

            switch (type)
            {
                case ".JPG":
                    mediaCopier = new ImageCopier(fInfo);
                    break;
                case ".MP4":
                case ".MOV":
                    mediaCopier = new MovieCopier(fInfo);
                    break;
                case ".NEF":
                    mediaCopier = new BitmapCopier(fInfo);
                    break;
                default:
                    break;
            }

            return mediaCopier;
        }
    }
}
