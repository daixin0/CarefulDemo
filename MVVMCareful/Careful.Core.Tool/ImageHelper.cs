using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Careful.Core.Tool
{
    public class ImageHelper
    {
        public static Boolean IsImage(string path)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                img.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static Bitmap GetBitmap(string path)
        {
            Bitmap bitmap = null;
            try
            {
                if (File.Exists(path))
                {
                    bitmap = new Bitmap(path);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return bitmap;
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        private static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);

            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
            srcBitmap.Dispose();
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitMap">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void CompressQuality(string sourceFile, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(GetBitmap(sourceFile), s, level);
            s.Close();
        }
        public static void CompressSize(string sourcefullname, int dispMaxWidth, int dispMaxHeight, string output)
        {
            try
            {
                Bitmap mg = new Bitmap(sourcefullname);
                System.Drawing.Size newSize = new System.Drawing.Size(dispMaxWidth, dispMaxHeight);
                Bitmap bp = ResizeImage(mg, newSize);
                if (bp != null)
                    bp.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);
                bp.Dispose();
            }
            catch(Exception ex)
            {
                var code = Marshal.GetLastWin32Error();
                throw new Exception(ex.Message + ":" + code.ToString(), ex);
            }
        }
        private static Bitmap ResizeImage(Bitmap mg, System.Drawing.Size newSize)
        {
            try
            {
                int x = 0;
                int y = 0;
                Bitmap bp;
                System.Drawing.Size thumbSize = new System.Drawing.Size((int)newSize.Width, (int)newSize.Height);
                bp = new Bitmap(newSize.Width, newSize.Height);
                x = (newSize.Width - thumbSize.Width) / 2;
                y = (newSize.Height - thumbSize.Height);
                System.Drawing.Graphics g = Graphics.FromImage(bp);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
                g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);
                mg.Dispose();
                return bp;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static BitmapImage GetBitmapImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.DecodePixelHeight = 20;
            bitmap.BeginInit();
            bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        public static BitmapSource GetPartImage(BitmapSource source, int XCoordinate, int YCoordinate, int Width, int Height)
        {
            BitmapSource bitmapSource = new CroppedBitmap(source, new Int32Rect(XCoordinate, YCoordinate, Width, Height));
            MemoryStream ms = new MemoryStream(ConvertToBytes(bitmapSource));
            BitmapSource result = BitmapFrame.Create(
                    ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnLoad);
            return result;
        }
        public static BitmapSource MakePicture(BitmapSource source1, BitmapSource source2)
        {
            Bitmap bitmap= AddBitmap(ConvertToBitmap(source1), ConvertToBitmap(source2));
            BitmapSource bitmapSource= ConvertToBitmapSource(bitmap);
            MemoryStream ms = new MemoryStream(ConvertToBytes(bitmapSource));
            BitmapSource result= BitmapFrame.Create(
                    ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnLoad);
            return result;
        }
        private static Bitmap AddBitmap(Bitmap first, Bitmap second)
        {
            var width = first.Width + second.Width;
            var height = first.Height >= second.Height ? first.Height : second.Height;
            Bitmap bitMap = new Bitmap(width, height);
            Graphics g1 = Graphics.FromImage(bitMap);
            g1.FillRectangle(System.Drawing.Brushes.White, new Rectangle(0, 0, width, height));
            g1.DrawImage(first, 0, 0, first.Width, first.Height);
            g1.DrawImage(second, first.Width, 0, second.Width, second.Height);
            first.Dispose();
            second.Dispose();
            return bitMap;
        }
        public static BitmapSource ConvertToBitmapSource(Bitmap btmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(btmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            MemoryStream ms = new MemoryStream(ConvertToBytes(bitmapSource));
            BitmapSource result = BitmapFrame.Create(
                    ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnLoad);
            return result;
        }
        private static byte[] ConvertToBytes(BitmapSource bitmapSource)
        {
            byte[] buffer = null;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);
            memoryStream.Position = 0;
            if (memoryStream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(memoryStream))
                {
                    buffer = br.ReadBytes((int)memoryStream.Length);
                }
            }
            memoryStream.Close();
            return buffer;
        }
        public static Bitmap ConvertToBitmap(BitmapSource source)
        {
            MemoryStream ms = new MemoryStream(ConvertToBytes(source));
            BitmapDecoder encoder = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnLoad);
            foreach (var item in encoder.Frames)
            {
                item.Freeze();
            }
            //encoder.Frames.Add(BitmapFrame.Create((BitmapSource)source));
            //encoder.Save(ms);

            Bitmap bp = new Bitmap(ms);
            ms.Close();
            return bp;
        }
    }
}
