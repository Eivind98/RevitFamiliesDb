using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RevitFamiliesDb.Objects
{
    public class DemElementType : DemElement
    {
        public string FamilyName { get; set; }
        public string ImagePath { get; set; }
        public BitmapImage Image
        {
            get
            {
                if (File.Exists(ImagePath))
                {
                    try
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.UriSource = new Uri(ImagePath);
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                    catch (Exception)
                    {

                    }
                }

                return null;
            }
        }


        public DemElementType()
        {


        }

        public DemElementType(ElementType element) : base(element)
        {
            FamilyName = element.FamilyName;
            //ImagePath = Global.TheDirPath + element.UniqueId + ".jpg";

            //System.Drawing.Size imgSize = new System.Drawing.Size(200, 200);

            //Bitmap bitmap = element.GetPreviewImage(imgSize);

            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            //encoder.Frames.Add(BitmapFrame.Create(System.Windows.Interop.Imaging
            //  .CreateBitmapSourceFromHBitmap(
            //    bitmap.GetHbitmap(),
            //    IntPtr.Zero,
            //    Int32Rect.Empty,
            //    BitmapSizeOptions.FromEmptyOptions())));
            //encoder.QualityLevel = 25;
            //FileStream file = new FileStream(ImagePath, FileMode.Create, FileAccess.Write);

            //encoder.Save(file);

            //file.Close();

            ImagePath = Path.Combine(Global.TheDirPath, element.UniqueId + ".jpg");
            System.Drawing.Size imgSize = new System.Drawing.Size(200, 200);

            using (Bitmap bitmap = element.GetPreviewImage(imgSize))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 25;

                    BitmapFrame frame = BitmapFrame.Create(System.Windows.Interop.Imaging
                        .CreateBitmapSourceFromHBitmap(
                            bitmap.GetHbitmap(),
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions()));

                    encoder.Frames.Add(frame);

                    encoder.Save(memoryStream);

                    using (FileStream file = new FileStream(ImagePath, FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.WriteTo(file);
                    }
                }
            }


        }



    }
}
