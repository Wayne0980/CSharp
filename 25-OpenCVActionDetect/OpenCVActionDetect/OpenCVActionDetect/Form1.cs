using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace OpenCVActionDetect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {  

            Image<Bgr, Byte> image1 = new Image<Bgr, Byte>("1.jpg");
            Image<Gray, Byte> gray1 = image1.Convert<Gray, Byte>();
            gray1 = gray1.ThresholdBinary(new Gray(100), new Gray(255));

            Image<Bgr, Byte> image2 = new Image<Bgr, Byte>("2.jpg");
            Image<Gray, Byte> gray2 = image2.Convert<Gray, Byte>();
            gray2 = gray2.ThresholdBinary(new Gray(100), new Gray(255));
                     
            double old_d = 0;
            //做差求绝对值  
         
                Image<Gray, Byte> gray3 = gray1.AbsDiff(gray2);
                gray3 = gray3.ThresholdBinary(new Gray(254), new Gray(255)).Dilate(1).Erode(1);
               
                    MemStorage storage = new MemStorage();
                    Contour<Point> contours2 = null;
                            
                for (
                        Contour<Point> contours = gray2.FindContours(
                        Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                        Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL,
                        storage);contours != null; contours = contours.HNext)
                    {
                        double areas = contours.Area;
                                    
                        if (old_d < areas)
                        {
                            old_d = areas;
                                       
                            contours2 = contours;
                        }
                                    
                    }
                if (contours2 != null && old_d > 1500)
                {
                   Rectangle BoundingBox = CvInvoke.cvBoundingRect(contours2,false);
                    CvInvoke.cvRectangle(image1, new Point(BoundingBox.X, BoundingBox.Y), new Point(BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), new MCvScalar(0, 0, 255, 255), 4, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
                }
                pictureBox1.Image = image1.ToBitmap(); 
        }
    }
}
