using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WatorWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int width;
        private readonly int height;

        private WriteableBitmap Bitmap;
        private byte[] Pixels;
        private DispatcherTimer Timer;
        private bool InStepMode;
        private World TheWorld;
        
        public MainWindow(int newWidth, int newHeight)
        {
            InitializeComponent();

            width = newWidth;
            height = newHeight;

            Bitmap = new(width, height, 96, 96, PixelFormats.Bgra32, null);
            SimDisplay.Source = Bitmap;

            Pixels = new byte[width * height * 4];
            TheWorld = new World(width, height);
            
            DrawFrame();

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value * 10);
            Timer.Tick += (s, e) =>
            {
                TheWorld.WorldTick();
                DrawFrame();
            };
            Timer.Start();
        }

        private void DrawFrame()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tile? tile = TheWorld.GetTile(x, y);
                    int pixelIndex = (y * width + x) * 4;

                    if (tile == null) // water
                    {
                        Pixels[pixelIndex + 0] = 255;
                        Pixels[pixelIndex + 1] = 0;
                        Pixels[pixelIndex + 2] = 0;
                    } 
                    else if (tile is Fish) // fish
                    {
                        Pixels[pixelIndex + 0] = 0;
                        Pixels[pixelIndex + 1] = 255;
                        Pixels[pixelIndex + 2] = 0;
                    } 
                    else // shark
                    {
                        Pixels[pixelIndex + 0] = 0;
                        Pixels[pixelIndex + 1] = 0;
                        Pixels[pixelIndex + 2] = 255;
                    }
                    Pixels[pixelIndex + 3] = 255;
                }
            }

            Bitmap.WritePixels(new Int32Rect(0, 0, width, height), Pixels, width * 4, 0);
        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Timer.IsEnabled)
            {
                TheWorld.WorldTick();
                DrawFrame();
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            TheWorld = new World(width, height);
            DrawFrame();
        }

        private void SpeedSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value * 10);
        }
    }
}