using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace photo_combination
{
    class Image_Fusion
    {
        /// <summary>
        /// 实现图像拼接
        /// </summary>
        /// <param name="im1">图像1</param>
        /// <param name="im2">图像2</param>
        /// <param name="com">com=x1-x2</param>
        /// <returns>拼接后的图像</returns>
        public static System.Drawing.Bitmap image_fusion(Image im1, Image im2, int com)
        {
            byte[] imdata1 = ImageDataConverter.ToByteArray((Bitmap)im1);
            byte[] imdata2 = ImageDataConverter.ToByteArray((Bitmap)im2);
            int newWidth = im1.Width + com;
            byte[] imdata_new = new byte[im1.Height * newWidth * 4];
            double scale = 0.5;

            GetValue(0, com, imdata1, imdata_new, newWidth, im1.Width, im1.Height);
            GetValue(im1.Width, newWidth, imdata2, imdata_new, newWidth, im1.Width, im1.Height);

            for (int y = 0; y < im1.Height; y++)
            {
                for (int x = com; x < im1.Width; x++)
                {
                    int i = x - com;
                    scale = 1 - ((double)(x - com) / (im1.Width - com - 1));
                    imdata_new[(y * newWidth + x) * 4] = (byte)(imdata1[(y * im1.Width + x) * 4] * scale
                                                                                +
                                                                imdata2[(y * im1.Width + i) * 4] * (1 - scale));

                    imdata_new[(y * newWidth + x) * 4 + 1] = (byte)(imdata1[(y * im1.Width + x) * 4 + 1] * scale
                                                                                        +
                                                                    imdata2[(y * im1.Width + i) * 4 + 1] * (1 - scale));

                    imdata_new[(y * newWidth + x) * 4 + 2] = (byte)(imdata1[(y * im1.Width + x) * 4 + 2] * scale
                                                                                       +
                                                                    imdata2[(y * im1.Width + i) * 4 + 2] * (1 - scale));

                    imdata_new[(y * newWidth + x) * 4 + 3] = 255;

                }
            }
            return ImageDataConverter.ToBitmap(imdata_new, newWidth, im1.Height);
        }



        /// 将两幅图中不重叠的部分的像素值赋给拼接图
        /// </summary>
        /// <param name="a"> 拼接图像中对应不重叠部分的起点</param>
        /// <param name="b"> 拼接图像中对应不重叠部分的终点</param>
        /// <param name="imagedata">原图像</param>
        /// <param name="new_imagedata">拼接图像</param>
        /// <param name="imageWidth">原图形宽</param>
        /// <param name="new_width">拼接图像宽</param>
        /// <param name="imageHeight">图像高</param>
        private static void GetValue(int a, int b, byte[] imagedata, byte[] new_imagedata, int new_width, int imageWidth, int imageHeight)
        {

            if (b <= imageWidth)
            {
                for (int j = 0; j < imageHeight; j++)
                {
                    for (int i = a; i < b; i++)
                    {
                        new_imagedata[(j * new_width + i) * 4] = imagedata[(j * imageWidth + i) * 4];
                        new_imagedata[(j * new_width + i) * 4 + 1] = imagedata[(j * imageWidth + i) * 4 + 1];
                        new_imagedata[(j * new_width + i) * 4 + 2] = imagedata[(j * imageWidth + i) * 4 + 2];
                        new_imagedata[(j * new_width + i) * 4 + 3] = imagedata[(j * imageWidth + i) * 4 + 3];
                    }
                }
            }

            else
            {
                int x = 0;
                for (int j = 0; j < imageHeight; j++)
                {
                    for (int i = a; i < b; i++)
                    {
                        //因为在上面调用时b=imageWidth +com，而x=i-com
                        x = i - (b - imageWidth);
                        new_imagedata[(j * new_width + i) * 4] = imagedata[(j * imageWidth + x) * 4];
                        new_imagedata[(j * new_width + i) * 4 + 1] = imagedata[(j * imageWidth + x) * 4 + 1];
                        new_imagedata[(j * new_width + i) * 4 + 2] = imagedata[(j * imageWidth + x) * 4 + 2];
                        new_imagedata[(j * new_width + i) * 4 + 3] = imagedata[(j * imageWidth + x) * 4 + 3];
                    }
                }
            }

        }
    }
}
