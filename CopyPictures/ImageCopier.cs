using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CopyPictures
{
    internal class ImageCopier : MediaCopier
    {
        public ImageCopier(FileInfo fInfo) : base(fInfo)
        {
        }

        public override DateTime GetDate()
        {
            DateTime date;

            using (FileStream fs = new FileStream(FInfo.FullName, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                if (myImage.PropertyIdList.Contains(36867))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = Encoding.UTF8.GetString(propItem.Value).Replace(":", "").Replace(" ", "").TrimEnd('\0');
                    date = DateTime.ParseExact(dateTaken, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                }
                else
                    date = FInfo.LastWriteTime;
            }

            return date;
        }
    }
}
