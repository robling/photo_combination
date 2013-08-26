using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace photo_combination
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 全局变量
        /// </summary>
        #region
        public static Image pic_im1;
        public static Image pic_im2;
        public static Image pic_im4;
        public static Image pic_im5;
        public static int[] fet_pot1;
        public static int[] fet_pot2;
        public static int com;
        public static int con;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 双击以大图显示拼接后图片
        /// 预留空间完成图片裁剪
        /// </summary>
        private void PictureBox3_DoubleClick(object sender, EventArgs e)
        {
            Form2 fr1 = new Form2();
            fr1.pictureBox1.Image = pictureBox3.Image;
            fr1.Show();
        }

        /// <summary>
        /// 打开图片
        /// </summary>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string name1;
            openFileDialog1.ShowDialog();
            name1 = openFileDialog1.FileName;
            if ("openFileDialog1" == name1)
            {
                MessageBox.Show("文件不能为空");
            }
            else
            {
                pictureBox1.Image = Image.FromFile(name1);
            }
            pic_im1 = pictureBox1.Image;
        }

        /// <summary>
        /// 打开图片
        /// </summary>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string name1;
            openFileDialog1.ShowDialog();
            name1 = openFileDialog1.FileName;
            if ("openFileDialog1" == name1)
            {
                MessageBox.Show("文件不能为空");
            }
            else
            {
                pictureBox2.Image = Image.FromFile(name1);
            }
            pic_im2 = pictureBox2.Image;
        }

        /// <summary>
        /// 启动柱面投影
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            //计时器
            Stopwatch timecount = new Stopwatch();
            timecount.Reset();//重设时间（清零）
            timecount.Start();//开始
            //定义线程并启动
            Thread pic_cc1 = new Thread(new ThreadStart(Cylinder_change.Cylinder_1));
            Thread pic_cc2 = new Thread(new ThreadStart(Cylinder_change.Cylinder_2));
            pic_cc1.Start();
            pic_cc2.Start();
            //检测线程，若已关闭则刷新主界面
            while(true)
            { 
                if(!pic_cc1.IsAlive&&!pic_cc2.IsAlive)
                {
                    pictureBox1.Image = Form1.pic_im1;
                    pictureBox2.Image = Form1.pic_im2;
                    timecount.Stop();//定时器结束
                    label5.Text = timecount.ElapsedMilliseconds.ToString();//显示运行时间。单位毫秒
                    break;
                }
            }
        }
        
        /// <summary>
        /// canny算子提取边缘点
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            //计时器
            Stopwatch timecount = new Stopwatch();
            timecount.Reset();//重设时间（清零）
            timecount.Start();//开始
            //定义线程并启动
            Thread pic_cc1 = new Thread(new ThreadStart(new_canny1));
            Thread pic_cc2 = new Thread(new ThreadStart(new_canny2));
            pic_cc1.Start();
            pic_cc2.Start();
            //检测线程，若已关闭则刷新主界面
            while (true)
            {
                if (!pic_cc1.IsAlive && !pic_cc2.IsAlive)
                {
                    pictureBox4.Image = Form1.pic_im1;
                    pictureBox5.Image = Form1.pic_im2;
                    timecount.Stop();//定时器结束
                    label6.Text = timecount.ElapsedMilliseconds.ToString();//显示运行时间。单位毫秒
                    break;
                }
            }
        }
        
        /// <summary>
        /// canny算子提取边缘点的算法代码
        /// </summary>
        #region
        private void new_canny1()
        {
            Canny CannyData1;
            CannyData1 = new Canny((Bitmap)pic_im1);
            Form1.pic_im1 = CannyData1.DisplayImage(CannyData1.EdgeMap);
        }
        private void new_canny2()
        {
            Canny CannyData2;
            CannyData2 = new Canny((Bitmap)pic_im2);
            Form1.pic_im2 = CannyData2.DisplayImage(CannyData2.EdgeMap);
        }
        #endregion

        /// <summary>
        /// 特征点提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //计时器
            Stopwatch timecount = new Stopwatch();
            timecount.Reset();//重设时间（清零）
            timecount.Start();//开始
            //定义线程并启动
            Thread pic_cc1 = new Thread(new ThreadStart(Feature_Extraction.capture1));
            Thread pic_cc2 = new Thread(new ThreadStart(Feature_Extraction.capture2));
            
            pic_im1 = pictureBox1.Image;
            pic_im4 = pictureBox4.Image;
            pic_im2 = pictureBox2.Image;
            pic_im5 = pictureBox5.Image;
            pic_cc2.Start();
            pic_cc1.Start();

            //检测线程，若已关闭则刷新主界面
            while (true)
            {
                if (!pic_cc1.IsAlive && !pic_cc2.IsAlive)
                {
                    //pictureBox4.Image = Form1.pic_im1;
                    //pictureBox5.Image = Form1.pic_im2;
                    timecount.Stop();//定时器结束
                    label7.Text = timecount.ElapsedMilliseconds.ToString();//显示运行时间。单位毫秒
                    break;
                }
            }
        }

        /// <summary>
        /// 匹配
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            //计时器
            Stopwatch timecount = new Stopwatch();
            timecount.Reset();//重设时间（清零）
            timecount.Start();//开始
            //定义线程并启动
            Thread pic_cc1 = new Thread(new ThreadStart(point_matching.matching));

            pic_im1 = pictureBox1.Image;
            pic_cc1.Start();

            //检测线程，若已关闭则刷新主界面
            while (true)
            {
                if (!pic_cc1.IsAlive)
                {
                    timecount.Stop();//定时器结束
                    label1.Text = com.ToString();
                    label2.Text = con.ToString();
                    label9.Text = timecount.ElapsedMilliseconds.ToString();//显示运行时间。单位毫秒
                    break;
                }
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = Image_Fusion.image_fusion(pic_im1, pic_im2, Math.Abs(com));
            //pictureBox3.Image.Save(@"C:\2.bmp");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //计时器
            Stopwatch timecount = new Stopwatch();
            timecount.Reset();//重设时间（清零）
            timecount.Start();//开始
            fet_pot1 = One_dimensional_median_filtering.median_filter(Form1.fet_pot1);
            fet_pot2 = One_dimensional_median_filtering.median_filter(Form1.fet_pot2);
            timecount.Stop();//定时器结束
            label8.Text = (1+ timecount.ElapsedMilliseconds).ToString();//显示运行时间。单位毫秒
        }                                 
    }

}

