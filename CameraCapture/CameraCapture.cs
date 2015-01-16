using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace CameraCapture
{
    public partial class CameraCapture : Form
    {
        public CameraCapture()
        {
            InitializeComponent();
        }

        private Capture capture;
        private bool captureInProgress;
        private HaarCascade haar;

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> ImageFrame = capture.QueryFrame();

            if (ImageFrame != null)
            {
                Image<Gray, byte> grayframe = ImageFrame.Convert<Gray, byte>();
                var faces = grayframe.DetectHaarCascade(haar, 1.2, 2, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(50, 50))[0];

                if (faces.Length > 0)
                {
                    foreach (var face in faces)
                    {
                        //int i=0;
                        ImageFrame.Draw(face.rect, new Bgr(Color.Red), 3);
                        //ImageFrame.Save("Face"+i.ToString()+".jpg");
                        label2.Text= "\n Face Detected : " + faces.Length.ToString();

                       // i = i + 1;
                        

                    }
                }
            }

            CamImageBox.Image = ImageFrame;
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                try
                {
                    capture = new Capture();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            if (capture != null)
            {
                if (captureInProgress)
                {
                    btnStart.Text = "Start";
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    btnStart.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }
        }

        private void ReleseData()
        {
            if (capture != null)
            {
                capture.Dispose();
            }
        }

        private void CameraCapture_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade.xml");
        }
    }
}
