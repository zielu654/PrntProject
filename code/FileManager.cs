using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace prntProject;
static internal class FileManager
{
    public static bool CompareBitmapsFast(Bitmap bmp1, Bitmap bmp2)
    {
        if (bmp1 == null || bmp2 == null)
            return false;
        if (object.Equals(bmp1, bmp2))
            return true;
        if (!bmp1.Size.Equals(bmp2.Size))//|| !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
            return false;

        int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

        bool result = true;
        byte[] b1bytes = new byte[bytes];
        byte[] b2bytes = new byte[bytes];

        BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
        BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

        Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
        Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

        for (int n = 0; n <= bytes - 1; n++)
        {
            if (b1bytes[n] != b2bytes[n])
            {
                result = false;
                break;
            }
        }

        bmp1.UnlockBits(bitmapData1);
        bmp2.UnlockBits(bitmapData2);

        return result;
    }
    public static string CheckFile(string filePath, string fileExtension)
    {
        bool correct = false;
        for (int i = 0; correct == false; i++)
        {
            if (File.Exists(filePath + (i > 0 ? $"({i})" : "") + fileExtension) == false)
            {
                correct = true;
                filePath += i > 0 ? $"({i})" : "";
            }
        }
        return filePath;
    }
    public static void CreateDirectory(string path)
    {
        // check if result directory exist (if not create)
        if (Directory.Exists(path + "results\\") == false)
            Directory.CreateDirectory(path + "results\\");
    }
    public static Bitmap GetBitmapFromFile(string path)
    {
        Image img = Image.FromFile(path);
        Bitmap downloadedImage = new(img);
        img.Dispose();
        return downloadedImage;
    }
}

