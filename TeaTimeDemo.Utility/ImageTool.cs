using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace TeaTimeDemo.Utility
{
    public class ImageTool
    {
        public static String compressImage(String bmpPath, int quality)
        {
            //原圖路徑
            Bitmap bmp = new Bitmap(bmpPath);
            ImageCodecInfo codecInfo = GetEncoder(bmp.RawFormat); //圖片編解碼信息
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter; //編碼器參數
            //壓縮圖路徑
            ImageFormat format = bmp.RawFormat;
            String newFilePath = String.Empty; //壓縮圖所在路徑
            // Guid.NewGuid().ToString()
            //GUID 是一個 128 位整數 （16 個字節），它可用於跨所有計算機和網絡中，任何唯一標識符是必需的。 此類標識符具有重複的可能性非常小
            String deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (format.Equals(ImageFormat.Jpeg))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".png";
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".bmp";
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".gif";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".icon";
            }
            else
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".jpg";
            }
            bmp.Save(newFilePath, codecInfo, encoderParameters); //保存壓縮圖
            return newFilePath; //返回壓縮圖路徑
        }

        private static ImageCodecInfo GetEncoder(ImageFormat rawFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == rawFormat.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static bool GetPicThumbnail(Stream stream, int dHeight, int dWidth, int flag, Stream outstream)
        {
            //可以直接从流里边得到图片,这样就可以不先存储一份了
            System.Drawing.Image iSource = System.Drawing.Image.FromStream(stream);

            //如果为参数为0就保持原图片
            if (dHeight == 0)
            {
                dHeight = iSource.Height;
            }
            if (dWidth == 0)
            {
                dWidth = iSource.Width;
            }


            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量 
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100 
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    //可以存储在流里边;
                    ob.Save(outstream, jpegICIinfo, ep);

                }
                else
                {
                    ob.Save(outstream, tFormat);
                }


                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
    }
}
