using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace photo_combination
{
    /// <summary>
    /// 特征点提取
    /// </summary>
    class Feature_Extraction
    {
        /// <summary>
        /// 特征点提取
        /// </summary>
        /// <param name="Width">图像宽度</param>
        /// <param name="Height">图像高度</param>
        /// <param name="Image">原图像对应的灰度图</param>
        /// <param name="Image2">边缘点组成的图形</param>
        /// <returns>特征点组成的图形</returns>
        public static void capture1()
        {
            Image im = Form1.pic_im1;
            Image im2 = Form1.pic_im4;
            int Width = im.Width;
            int Height = im.Height;
            byte[] Image = ImageDataConverter.ToByteArray((Bitmap)im);
            byte[] Image2 = ImageDataConverter.ToByteArray((Bitmap)im2);
            /*把RGB数组image[]改为灰度图像数组*/
            int norm;
            byte[] Image3 = new byte[Width * Height];
            double Maxgrads = 0;//梯度最大值
            int[] featurespot = new int[Width];//特征点纵坐标数组
            int margin;//梯度值的点与中间点间距
            int Minmargin;//最小间距
            Image = Re(Image, Width, Height);
            Image2 = Re(Image2, Width, Height);


            //特征点梯度
            #region
            for (int i = 0; i < Width; i++)
            {
                norm = 0;
                Maxgrads = 0;
                for (int j = 1; j < Height - 1; j++)
                {
                    //判断是否为边缘点
                    if (Image2[i + j * Width] == 255)
                    {
                        //
                        norm = 1;
                        //计算边缘点的梯度
                        if (solve(i, j * Width, Image, Width) > Maxgrads)
                        {
                            Maxgrads = solve(i, j * Width, Image, Width);

                            Image2[i + j * Width] = (byte)Maxgrads;

                            Image2[i + featurespot[i] * Width] = 0;

                            featurespot[i] = j;
                        }
                        //其余非边缘点梯度值赋0值
                        else
                            Image2[i + j * Width] = 0;
                    }


                }
                //如果本列无边缘点则进入该步
                if (norm == 0)
                {
                    Maxgrads = 0;

                    for (int k = Width; k < Width * (Height - 1); k = k + Width)
                    {
                        if (solve(i, k, Image, Width) >= Maxgrads)
                        {
                            Maxgrads = solve(i, k, Image, Width);
                            Image2[i + k] = (byte)Maxgrads;
                            Image2[i + featurespot[i] * Width] = 0;
                            featurespot[i] = k / Width;
                        }
                        else
                            Image2[i + k] = 0;
                    }


                }
            }

            for (int i = 0; i < Width; i++)
            {
                Minmargin = Height / 2;
                margin = Height / 2;
                for (int j = 0; j < Width * Height; j = j + Width)
                {
                    if (Image2[i + j] != 0)
                    {
                        margin = Math.Abs(j / Width - Height / 2);
                        if (Minmargin > margin)
                        {
                            Minmargin = margin;
                            featurespot[i] = j / Width;
                        }
                    }
                }
            }
            #endregion
            Form1.fet_pot1 = featurespot;

           
        }
        public static void capture2()
        {
            Image im = Form1.pic_im2;
            Image im2 = Form1.pic_im5;
            int Width = im.Width;
            int Height = im.Height;
            byte[] Image = ImageDataConverter.ToByteArray((Bitmap)im);
            byte[] Image2 = ImageDataConverter.ToByteArray((Bitmap)im2);
            /*把RGB数组image[]改为灰度图像数组*/
            int norm;
            byte[] Image3 = new byte[Width * Height];
            double Maxgrads = 0;//梯度最大值
            int[] featurespot = new int[Width];//特征点纵坐标数组
            int margin;//梯度值的点与中间点间距
            int Minmargin;//最小间距
            Image = Re(Image, Width, Height);
            Image2 = Re(Image2, Width, Height);


            //特征点梯度
            #region
            for (int i = 0; i < Width; i++)
            {
                norm = 0;
                Maxgrads = 0;
                for (int j = 1; j < Height - 1; j++)
                {
                    //判断是否为边缘点
                    if (Image2[i + j * Width] == 255)
                    {
                        //
                        norm = 1;
                        //计算边缘点的梯度
                        if (solve(i, j * Width, Image, Width) > Maxgrads)
                        {
                            Maxgrads = solve(i, j * Width, Image, Width);

                            Image2[i + j * Width] = (byte)Maxgrads;

                            Image2[i + featurespot[i] * Width] = 0;

                            featurespot[i] = j;
                        }
                        //其余非边缘点梯度值赋0值
                        else
                            Image2[i + j * Width] = 0;
                    }


                }
                //如果本列无边缘点则进入该步
                if (norm == 0)
                {
                    Maxgrads = 0;

                    for (int k = Width; k < Width * (Height - 1); k = k + Width)
                    {
                        if (solve(i, k, Image, Width) >= Maxgrads)
                        {
                            Maxgrads = solve(i, k, Image, Width);
                            Image2[i + k] = (byte)Maxgrads;
                            Image2[i + featurespot[i] * Width] = 0;
                            featurespot[i] = k / Width;
                        }
                        else
                            Image2[i + k] = 0;
                    }


                }
            }

            for (int i = 0; i < Width; i++)
            {
                Minmargin = Height / 2;
                margin = Height / 2;
                for (int j = 0; j < Width * Height; j = j + Width)
                {
                    if (Image2[i + j] != 0)
                    {
                        margin = Math.Abs(j / Width - Height / 2);
                        if (Minmargin > margin)
                        {
                            Minmargin = margin;
                            featurespot[i] = j / Width;
                        }
                    }
                }
            }
            #endregion
            Form1.fet_pot2 = featurespot;
        }

        //梯度公式
        public static double solve(int i, int j, byte[] Image, int Width)
        {
            double grads;
            grads = (Image[i + j + 1] - Image[i + j - 1]) * (Image[i + j + 1] - Image[i + j - 1]) / 4 + Math.Sqrt(Image[i + j + Width] - Image[i + j - Width]);
            return grads;
        }

        /// <summary>
        /// 灰度化图像
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static byte[] Re(byte[] Image, int Width, int Height)
        {
            byte[] Gray = new byte[Width * Height];
            for (int i = 0; i < Width * Height; i++)
            {
                Gray[i] = (byte)(Image[i * 4] * 0.3 + Image[i * 4 + 1] * 0.59 + Image[i * 4 + 2] * 0.11);
            }
            return Gray;
        }
    }
}
