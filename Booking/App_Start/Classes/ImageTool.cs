using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace Classes
{
    public class ImageTool
    {
        public ArrayList FetchLinksFromSource(string htmlSource)
        {
            ArrayList links = new ArrayList();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(href);
            }
            return links;
        }
        public string GetImageUrlFromContent(string source, ref string fimgname)
        {
            string temp = source;
            string s2 = "";
            if (temp.Length > 0)
            {
                int i = temp.IndexOf("<img");
                if (i >= 0)
                {
                    string s = temp.Substring(i, temp.Length - i);
                    int j = s.IndexOf(">");
                    if (j >= 0)
                    {
                        s2 = s.Substring(0, j);
                        int k = s2.IndexOf("src=");
                        if (k >= 0)
                        {
                            string s3 = s2.Substring(k + 5, s2.Length - (k + 5));
                            string ext = "";
                            int t = GetIndex(s3, ref ext);
                            if (t >= 0)
                            {
                                string sss = s3.Substring(0, t) + ext;
                                fimgname = Guid.NewGuid() + "." + ext;
                                return sss;
                            }
                        }
                    }
                }

            }
            return s2;
        }
        private static int GetIndex(string s, ref string ext)
        {
            string[] ss = new string[] { "png", "jpeg", "jpg", "gif", "tif", "bmp" };
            foreach (var s1 in ss)
            {
                int i = s.LastIndexOf(s1);
                if (i >= 0)
                {
                    ext = s1;
                    return i;
                }
            }
            return -1;
        }

        public static void CropAndOverwrite(string imgPath, int x1, int y1, int height, int width)
        {

            //Create a rectanagle to represent the cropping area
            Rectangle rect = new Rectangle(x1, y1, width, height);
            //see if path if relative, if so set it to the full path
           
            //Load the original image
            Bitmap bMap = new Bitmap(imgPath);
            //The format of the target image which we will use as a parameter to the Save method
            var format = bMap.RawFormat;


            //Draw the cropped part to a new Bitmap
            var croppedImage = bMap.Clone(rect, bMap.PixelFormat);

            //Dispose the original image since we don't need it any more
            bMap.Dispose();
            //Remove the original image because the Save function will throw an exception and won't Overwrite by default
            if (System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);

            //Save the result in the format of the original image
            croppedImage.Save(imgPath, format);
            //Dispose the result since we saved it
            croppedImage.Dispose();
        }
        public static MemoryStream CropToStream(string path, int x, int y, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            Rectangle fromRectangle = new Rectangle(x, y, width, height);
            using (Image image = Image.FromFile(path, true))
            {
                Bitmap target = new Bitmap(fromRectangle.Width, fromRectangle.Height);
                using (Graphics g = Graphics.FromImage(target))
                {
                    Rectangle croppedImageDimentions = new Rectangle(0, 0, target.Width, target.Height);
                    g.DrawImage(image, croppedImageDimentions, fromRectangle, GraphicsUnit.Pixel);
                }
                MemoryStream stream = new MemoryStream();
                target.Save(stream, image.RawFormat);                
                stream.Position = 0;
                return stream;
            }
        }
        public static bool SaveCroppedImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, jpgInfo, encParams);
                    return true;
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return false;
        }

        public static string GetNewPathForDupes(string path, ref string fn)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;
            string newFullPath = path;
            string new_file_name = filename + extension;
            while (System.IO.File.Exists(newFullPath))
            {
                string newFilename = string.Format("{0}({1}){2}", filename, counter, extension);
                new_file_name = newFilename;
                newFullPath = Path.Combine(directory, newFilename);
                counter++;
            };
            fn = new_file_name;
            return newFullPath;
        }
    }
}