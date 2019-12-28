using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CopyPictures
{
    internal class BitmapCopier : MediaCopier
    {
        public BitmapCopier(FileInfo fInfo) : base(fInfo)
        {
        }
        public override DateTime GetDate()
        {
            DateTime date;

            using (FileStream fs = new FileStream(FInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                date = DateTime.Parse(md.DateTaken);
            }

            return date;
        }
    }
}
