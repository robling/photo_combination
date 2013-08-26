using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace photo_combination
{
    class point_matching
    {
        /// <summary>
        /// 找到最佳匹配位置
        /// </summary>
        /// <param name="mag1">左图每列梯度值最大的纵坐标</param>
        /// <param name="mag2">右图每列梯度值最大的纵坐标</param>
        /// <param name="x1">左图最佳匹配位置的横坐标</param>
        /// <param name="y1">左图最佳匹配位置的纵坐标</param>
        /// <param name="x2">右图最佳匹配位置的横坐标</param>
        /// <param name="y2">右图最佳匹配位置的纵坐标</param>
        /// <param name="imageWidth">图度像宽</param 
        /// <param name="imageHeight">图像高度</param>
        public static void matching(/*int[] mag1, int[] mag2, ref int x1, ref int y1, ref int x2, ref int y2, int imageWidth, int imageHeight*/)
        {
            int[] mag1 = Form1.fet_pot1;
            int[] mag2 = Form1.fet_pot2;
            int imageWidth = Form1.pic_im1.Width;
            int imageHeight = Form1.pic_im1.Height;
            int x1 = 0;
          
            int x2 = 0;
 
            int Length = imageWidth / 10;
            //S1中的元素为对应Simial中方差的左图像中的横坐标
            List<int> S1 = new List<int>();
            //S2中的元素为对应Simial中方差的右图像中的横坐标
            List<int> S2 = new List<int>();
            //s1,s2为长度为L的取自mag1[],mag2[]的子数组
            List<int> s1 = new List<int>();
            List<int> s2 = new List<int>();

            //SE中的元素为s1与所有s2的方差
            List<double> SE = new List<double>();
            //Simial里的元素为右边图像中与左边图像中一s1最为相似的s2之间的方差值
            List<double> Simil = new List<double>();

            int i, j, m, n;


            for (i = imageWidth / 2; i < imageWidth - Length; i++)
            {

                s1.Clear();//将s1清零
                for (m = 0; m < Length; m++)
                {
                    s1.Add(mag1[i + m]);
                }

                for (j = 0; j < imageWidth / 2 - Length; j++)
                {
                    s2.Clear();
                    for (n = 0; n < Length; n++)
                    {
                        s2.Add(mag2[j + n]);
                    }
                    if (i == 400 && j == 59)
                    {
                        j = 59;
                    }
                    //GetVariance 函数得到s1,s2的方差，并储存到数组SE中
                    SE.Add(GetVariance(s1, s2));
                }
                //GetMin函数得到SE数组中的最小方差值，并得到与此对应的右图中的横坐标x2
                Simil.Add(GetMin(SE, ref x2));
                //左图对应的横坐标储存到S1中
                S1.Add(i);
                //右图对应的横坐标储存到S2中
                S2.Add(x2);
                SE.Clear();
            }

            FinalMin(Simil, S1, S2, ref x1, ref x2);

            int k = 0;
            int temp = 0;
            for (m =x1 ,n=x2 ; m < Length +x1  ; m++,n++)
            {

                if (mag1 [m] >0 &&  mag2[n ]> 0)            
                {
                    k++;
                    temp = Math.Abs(mag1[m] - mag2[n]);
                }
            }
            Form1.con = temp / k;
  
            Form1.com = (x1 - x2);
        }
            

        /// <summary>
        /// 得到s2,s2的方差
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns> 
        private static double GetVariance(List<int> s1, List<int> s2)
        {
            
             int k = 0;
             int temp = 0;
             double a = 0;
             double mean=0;
             int i;
            
            
            //应对无需滤波的情况
            #region
           // for (p = 0; p < s1.Count; p++)
           // {
           //     mean += (s1[p] - s2[p]) / (double)s1.Count;
           // }
           //// mean += mean / s1.Count;

           // for (p= 0; p < s1.Count; p++)
           // {
           //     a += (double)((s1[p] - s2[p] - mean) * (s1[p] - s2[p] - mean));
           // }
            #endregion
            //应对滤波后的情况
            #region
            for (i = 0; i < s1.Count; i++)
            {
                if (s1[i] > 0 && s2[i] > 0)
                {
                    k++;
                    temp += s1[i] - s2[i];
                }
            }
            if (k > 0)
            {
                mean = ((double )temp / k);
                for (i = 0; i < s1.Count; i++)
                {
                    if (s1[i] > 0 && s2[i] > 0)
                    {
                        a += (double)((s1[i] - s2[i] - mean) * (s1[i] - s2[i] - mean));
                    }
                }
            }
            else
            {
                a = 10000;
            }
            #endregion



            return a;
        }


        /// <summary>
        /// 右图中的所有s2与左图s1的所有方差的最小值
        /// </summary>
        /// <param name="se"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        private static double GetMin(List<double> se, ref int x2)
        {
            double a = se[0];
            for (int i = 0; i < se.Count; i++)
            {
                if (se[i] < a)
                {
                    a = se[i];
                    x2 = i;
                }
            }
            return a;
        }


        /// <summary>
        ///全图中的最小方差
        /// </summary>
        /// <param name="simil"></param>
        /// <param name="s1"></param>储存左图中每一个s1对应最小方差的横坐标的数组
        /// <param name="s2"></param>储存右图中对应每一个s1最小方差的横坐标的数组
        /// <param name="x1"></param>全图最小方差对应的左图中的横坐标
        /// <param name="x2"></param>全图最小方差对应的右图中的横坐标
        private static void FinalMin(List<double> simil, List<int> s1, List<int> s2, ref int x1, ref int x2)
        {
            double a = simil[0];
            for (int i = 0; i < simil.Count; i++)
            {
                if (simil[i] < a)
                {
                    a = simil[i];
                    x1 = s1[i];
                    x2 = s2[i];
                }
            }

        }
    }
}
