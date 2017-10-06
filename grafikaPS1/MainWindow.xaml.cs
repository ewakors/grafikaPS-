using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace grafikaPS1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
 
        public Rectangle change { get; set; }
        private enum SelectedShape
        { None, Line, Ellipse, Rectangle }

        private SelectedShape Shape1 = SelectedShape.None;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void addEllipse_Click(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Ellipse;
            Ellipse myEllipse = new Ellipse();

            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.Width = int.Parse(widthTxt.Text);
            myEllipse.Height = int.Parse(heightTxt.Text);

            canvasArea.Children.Add(myEllipse);
        }

        private void addLine_Click(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Line;

            Line myLine = new Line();

            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = 1;
            myLine.X2 = 50;
            myLine.Y1 = 1;
            myLine.Y2 = 50;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;

            myLine.StrokeThickness = int.Parse(ticknessTxt.Text);
            canvasArea.Children.Add(myLine);
        }

        private void addRectengle_Click(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Rectangle;
            Canvas canvas = new Canvas();
            Rectangle myRectangle = new Rectangle();

            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myRectangle.Fill = mySolidColorBrush;
            myRectangle.StrokeThickness = int.Parse(ticknessTxt.Text);
            myRectangle.Stroke = Brushes.Black;

            myRectangle.Width = int.Parse(widthTxt.Text);
            myRectangle.Height = int.Parse(heightTxt.Text);

            canvasArea.Children.Add(myRectangle);
        }

        private void canvasArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Shape Rendershape = null;

            switch (Shape1)
            { 
                case SelectedShape.Line:
                    Rendershape = new Line() { X1 = 1, X2 = 50, Y1 = 1, Y2 = 50, StrokeThickness = int.Parse(ticknessTxt.Text) };
                    Rendershape.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                    Rendershape.HorizontalAlignment = HorizontalAlignment.Left;
                    Rendershape.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case SelectedShape.Ellipse:
                    Rendershape = new Ellipse() { Height = int.Parse(heightTxt.Text), Width = int.Parse(widthTxt.Text) };
                    RadialGradientBrush brush = new RadialGradientBrush();
                    brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF7689"), 0.250));
                    brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF7689"), 0.100));
                    brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF7689"), 8));
                    Rendershape.Fill = brush;
                    break;
                case SelectedShape.Rectangle:
                    Rendershape = new Rectangle() { Fill = Brushes.Blue, Height = int.Parse(heightTxt.Text), Width = int.Parse(widthTxt.Text) };
                    break;
                default:
                    return;
            }

            Canvas.SetLeft(Rendershape, e.GetPosition(canvasArea).X);
            Canvas.SetTop(Rendershape, e.GetPosition(canvasArea).Y);

            canvasArea.Children.Add(Rendershape);

        }

        private void canvasArea_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((Canvas)sender);
            HitTestResult result = VisualTreeHelper.HitTest(canvasArea, pt);

            if (result != null)
            {
                canvasArea.Children.Remove(result.VisualHit as Shape);
            }
        }

        public void YourGotFocusEvent(object sender, RoutedEventArgs e)
        {
            //heightTxt.Text = string.Empty;
            //// if you want this to happen only the first time you can remove the event handler like this
            //heightTxt.GotFocus -= YourGotFocusEvent;
        }
    }
}
