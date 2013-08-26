using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace photo_combination
{
    static class ImageDataConverter
    {

        #region convert to data
        /// <summary>
        /// 将Bitmap像素强制转化为真彩像素(4 byte: blue,green,red,alpha),存为byte型1维数组.
        /// </summary>
        /// <param name="sourceBitmap">源图像</param>
        /// <returns>byte型1维数组,size=width*height*4.</returns>
        public static byte[] ToByteArray(System.Drawing.Bitmap sourceBitmap)
        {
            if (sourceBitmap == null)
                return null;

            int width = sourceBitmap.Width;
            int height = sourceBitmap.Height;
            System.Drawing.Imaging.BitmapData bData = sourceBitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //获取数据的指针
            System.IntPtr ptr = bData.Scan0;

            //数组大小
            int size = bData.Stride * height;
            ///////////////////////////////////////////////
            byte[] byteData = new byte[size];

            //copy
            System.Runtime.InteropServices.Marshal.Copy(ptr, byteData, 0, size);

            sourceBitmap.UnlockBits(bData);

            return byteData;
        }


        /// <summary>
        /// 将Bitmap像素强制转化为真彩像素(4 double: blue,green,red,alpha),存为double型1维数组.
        /// </summary>
        /// <param name="sourceBitmap">源图像</param>
        /// <returns>double型1维数组,size=width*height*4.</returns>
        public static double[] ToDoubleArray(System.Drawing.Bitmap sourceBitmap)
        {
            if (sourceBitmap == null)
                return null;

            byte[] byteData = ToByteArray(sourceBitmap);

            double[] data = new double[byteData.Length];
            //把byte数组转换成double数组
            int i = 0;
            foreach (byte bv in byteData)
                data[i++] = (double)bv;

            return data;
        }

        /// <summary>
        /// Convert a Bitmap to(4 data: blue, green, red, alpha),store to a data array
        /// </summary>
        /// <typeparam name="T">Whatever type you like</typeparam>
        /// <param name="sourceBitmap">Source bitmape</param>
        /// <param name="array">The output array</param>
        public static void ToArray<T>(System.Drawing.Bitmap sourceBitmap, out T[] array)
            where T : IConvertible
        {
            array = null;

            if (sourceBitmap == null)
                return;

            int width = sourceBitmap.Width;
            int height = sourceBitmap.Height;
            System.Drawing.Imaging.BitmapData bData = sourceBitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //获取数据的指针
            System.IntPtr ptr = bData.Scan0;

            //数组大小
            int size = bData.Stride * height;
            ///////////////////////////////////////////////
            byte[] byteData = new byte[size];

            //copy
            System.Runtime.InteropServices.Marshal.Copy(ptr, byteData, 0, size);

            sourceBitmap.UnlockBits(bData);

            //Convert type
            array = new T[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = (T)Convert.ChangeType(byteData[i], typeof(T));
            }
        }
        #endregion

        #region convert to bimap

        /// <summary>
        /// 将1维范型真彩像素(4 T: blue,green,red,alpha),或灰度像素(1 T: gray)强制转化为真彩Bitmap.
        /// </summary>
        /// <param name="source1D">1维数组</param>
        /// <param name="width">图像宽</param>
        /// <param name="height">图像高</param>
        /// <returns>格式为Format32bppArg的图像</returns>
        public static System.Drawing.Bitmap ToBitmap<T>(T[] source1D, int width, int height)
            where T : IConvertible
        {
            System.Drawing.Bitmap bitmap = null;
            if (source1D != null)
            {
                //将T类型转换为byte类型
                byte[] byteSource1D = new byte[source1D.Length];
                for (int i = 0; i < source1D.Length; ++i)
                    byteSource1D[i] = (byte)Convert.ChangeType(source1D[i], typeof(byte));

                byte[] colorData = UniformToColorArray(byteSource1D, width, height);

                bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                System.Drawing.Imaging.BitmapData bData = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, width, height),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);    //lock

                //Get the piont of destBitmap
                System.IntPtr ptr = bData.Scan0;

                //copy data to destBitmap
                System.Runtime.InteropServices.Marshal.Copy(colorData, 0, ptr, colorData.Length);

                bitmap.UnlockBits(bData);   //unlock
            }
            return bitmap;
        }

        private static byte[] UniformToColorArray(byte[] sourceData, int width, int height)
        {
            byte[] colorData = null;
            if (IsGrayArray(sourceData, width, height))
                colorData = GrayToRgb(sourceData, width, height);
            else if (IsColorArray(sourceData, width, height))
                colorData = sourceData;
            else
                colorData = null;
            return colorData;
        }

        private static bool IsGrayArray(byte[] sourceData, int width, int height)
        {
            if (sourceData.Length == width * height)
                return true;
            else
                return false;
        }

        private static bool IsColorArray(byte[] sourceData, int width, int height)
        {
            if (sourceData.Length == width * height * 4)
                return true;
            else
                return false;
        }

        private static byte[] GrayToRgb(byte[] grayData, int width, int height)
        {
            byte[] colorData = new byte[grayData.Length * 4];
            int indexColor = 0;
            int indexGray = 0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    indexGray = y * width + x;
                    indexColor = indexGray * 4;
                    colorData[indexColor] =
                    colorData[indexColor + 1] =
                    colorData[indexColor + 2] = grayData[indexGray];
                    colorData[indexColor + 3] = 255;
                }
            return colorData;
        }

        /// <summary>
        /// 将2维范型真彩像素(4 T: blue,green,red,alpha),或灰度像素(1 T: gray)强制转化为真彩Bitmap.
        /// </summary>
        /// <param name="source2D">2维数组</param>
        /// <param name="width">图像宽</param>
        /// <param name="height">图像高</param>
        /// <returns>格式为Format32bppArg的图像</returns>
        public static System.Drawing.Bitmap ToBitmap<T>(T[,] source2D, int width, int height)
            where T : IConvertible
        {
            System.Drawing.Bitmap bitmap = null;

            if (source2D != null)
            {
                T[] data1D = ConvertArrayOf2DimentionTo1(ref source2D);
                bitmap = ToBitmap(data1D, width, height);
            }

            return bitmap;
        }

        private static T[] ConvertArrayOf2DimentionTo1<T>(ref T[,] array2D)
        {
            int height = array2D.GetLength(0);
            int width = array2D.GetLength(1);
            T[] array1D = new T[height * width];

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    array1D[y * width + x] = array2D[y, x];
                }
            return array1D;
        }

        #endregion convert to bitmap
    }
}
