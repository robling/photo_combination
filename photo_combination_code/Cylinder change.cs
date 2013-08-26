using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace photo_combination
{
    class Cylinder_change
    {
        public static void Cylinder_1()
        {
            Image im = Form1.pic_im1;
            //图片的张角
            double angle = 0.69813170079773183076947630739545;

            //图像转数据
            byte[] imagedata = ImageDataConverter.ToByteArray((Bitmap)im);

            //获得图像的宽高
            int width = im.Width;
            int height = im.Height;

            //新图像的数据
            byte[] imagedata_std = new byte[width * height * 4];

            //变换后的地址
            int[] pointtemp = new int[2];

            int i = 0, j = 0, k = 0;
            //循环进行地址变换

            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    pointtemp = Cylinder_change.point_change(width, height, i, j, angle);
                    for (k = 0; k < 4; k++)
                    {
                        imagedata_std[(pointtemp[1] * width + pointtemp[0]) * 4 + k] = imagedata[(j * width + i) * 4 + k];
                    }
                }
            }
            im = ImageDataConverter.ToBitmap(imagedata_std, width, height);
            Form1.pic_im1 = im;
            //im.Save(@"D:\2.bmp");
            //Form1.isalived = true;
        }
        public static void Cylinder_2()
        {
            Image im = Form1.pic_im2;
            //图片的张角
            double angle = 0.69813170079773183076947630739545;

            //图像转数据
            byte[] imagedata = ImageDataConverter.ToByteArray((Bitmap)im);

            //获得图像的宽高
            int width = im.Width;
            int height = im.Height;

            //新图像的数据
            byte[] imagedata_std = new byte[width * height * 4];

            //变换后的地址
            int[] pointtemp = new int[2];

            int i = 0, j = 0, k = 0;
            //循环进行地址变换

            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    pointtemp = Cylinder_change.point_change(width, height, i, j, angle);
                    for (k = 0; k < 4; k++)
                    {
                        imagedata_std[(pointtemp[1] * width + pointtemp[0]) * 4 + k] = imagedata[(j * width + i) * 4 + k];
                    }
                }
            }
            im = ImageDataConverter.ToBitmap(imagedata_std, width, height);
            Form1.pic_im2 = im;
        }

        /// <summary>
        /// 柱面投影中某一特定点的坐标变换
        /// </summary>
        /// <param name="Width">图像宽度</param>
        /// <param name="Height">图像高度</param>
        /// <param name="x">特定点的横坐标</param>
        /// <param name="y">特定点的纵坐标</param>
        /// <param name="agl1">图像的张角</param>
        /// <returns></returns>
        public static int[] point_change(int Width, int Height, int x, int y, double agl1)
        {
            //用于传出的新坐标点
            int[] Coordinate_new = new int[2];
            
            double temp_r;
            double temp_k;

            temp_r = Width / (2 * Math.Tan(agl1 / 2));
            temp_k = Math.Sqrt(temp_r * temp_r + (Width / 2 - x) * (Width / 2 - x));

            //公式
            Coordinate_new[0] = (int)(temp_r * (agl1 / 2) + temp_r * Math.Atan((x - Width / 2) / temp_r));
            Coordinate_new[1] = (int)(Height / 2 + (temp_r * (y - Height / 2) / temp_k));


            //不进行变换的方式
            //Coordinate_new[0] = x;
            //Coordinate_new[1] = y;

            return Coordinate_new;
        }
    }
}
