using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Image_Stitching_Emgucv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStitch_Click(object sender, RoutedEventArgs e)
        {
            //Change this file path to the path where the images you want to stich are located
            string filePath = @"./Images/";

            //Read all the images from the path and store them in a list
            List<Mat> images = ImageStitching.GetImages(filePath);

            //Stitch the images that are located in the list
            Mat output = ImageStitching.StichImages(images);

            //Cath the error if the stitch image is null
            try
            {
                //Convert the Mat object to a bitmap
                Bitmap img = output.ToBitmap();

                //Using the method below, convert the bitmap to an imagesource
                imgOutput.Source = ImageSourceFromBitmap(img);
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("The images in the folder either couldn't be stitched, or the folder is empty");
            }
        }

        //Convert a bitmap to imagesource
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
    }
}