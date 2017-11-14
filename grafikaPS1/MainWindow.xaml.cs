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
        bool isDown, isDragging, isSelected;
        UIElement selectedElement = null;
        double originalLeft, originalTop, x1, y1, skala = 1.0;
        int stopnie;
        Point startPoint;
        RotateTransform rotation;
        ScaleTransform scal;
        TransformGroup transformation;

        AdornerLayer adornerLayer;

        public Rectangle change { get; set; }

        private enum SelectedShape
        { None, Line, Ellipse, Rectangle }

        private SelectedShape Shape1 = SelectedShape.None;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //registering mouse events
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeave += MainWindow_MouseLeave;

            canvasArea.PreviewMouseLeftButtonDown += MyCanvas_PreviewMouseLeftButtonDown;
            canvasArea.PreviewMouseLeftButtonUp += MyCanvas_PreviewMouseLeftButtonUp;
        }

        private void StopDragging()
        {
            if (isDown)
            {
                isDown = isDragging = false;
            }
        }

        private void zerowanieMacierzy()
        {
            transformation = new TransformGroup();
            rotation = new RotateTransform(0, x1, y1);
            transformation.Children.Add(rotation);
            scal = new ScaleTransform(skala, skala);
            transformation.Children.Add(scal);
        }

        private void MyCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        private void MyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            skala = 1.0;
            stopnie = 0;
            //removing selected element
            if (isSelected)
            {
                isSelected = false;
               
                if (selectedElement != null)
                {
                    zerowanieMacierzy();
                    adornerLayer.Remove(adornerLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }

            // select element if any element is clicked other then canvas
            if (e.Source != canvasArea)
            {
                isDown = true;
                startPoint = e.GetPosition(canvasArea);

                selectedElement = e.Source as UIElement;

                originalLeft = Canvas.GetLeft(selectedElement);
                originalTop = Canvas.GetTop(selectedElement);

                //adding adorner on selected element
                adornerLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                adornerLayer.Add(new SimpleCircleAdorner(selectedElement));
                isSelected = true;
                e.Handled = true;
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            //handling mouse move event and setting canvas top and left value based on mouse movement
            if (isDown)
            {
                if ((!isDragging) &&
                    ((Math.Abs(e.GetPosition(canvasArea).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(canvasArea).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    isDragging = true;

                if (isDragging)
                {
                    Point position = Mouse.GetPosition(canvasArea);
                    Canvas.SetTop(selectedElement, position.Y - (startPoint.Y - originalTop));
                    Canvas.SetLeft(selectedElement, position.X - (startPoint.X - originalLeft));
                }
            }
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            //stop dragging on mouse leave
            StopDragging();
            e.Handled = true;
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //stop dragging on mouse left button up
            StopDragging();
            e.Handled = true;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //remove selected element on mouse down
            if (isSelected)
            {
                isSelected = false;
                if (selectedElement != null)
                {
                    adornerLayer.Remove(adornerLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
        }

        private void addEllipse_Click(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Ellipse;
        }

        private void addLine_Click(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Line;
        }

        private void addRectengle_Click(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Rectangle;
        }

        private void canvasArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Shape Rendershape = null;
            
            {
                switch (Shape1)
                {
                    case SelectedShape.Line:
                        if ((int.Parse(x1Txt.Text) > 0) && (int.Parse(x2Txt.Text) > 0) && (int.Parse(y1Txt.Text) > 0) && (int.Parse(y2Txt.Text) > 0 ) && (int.Parse(ticknessTxt.Text) > 0 ))
                        {
                            Rendershape = new Line() { X1 = int.Parse(x1Txt.Text), X2 = int.Parse(x2Txt.Text), Y1 = int.Parse(y1Txt.Text), Y2 = int.Parse(x2Txt.Text), StrokeThickness = int.Parse(ticknessTxt.Text) };
                            Rendershape.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                            Rendershape.HorizontalAlignment = HorizontalAlignment.Left;
                            Rendershape.VerticalAlignment = VerticalAlignment.Center;
                            Canvas.SetLeft(Rendershape, e.GetPosition(canvasArea).X);
                            Canvas.SetTop(Rendershape, e.GetPosition(canvasArea).Y);
                            canvasArea.Children.Add(Rendershape);

                        }
                        else
                        {
                            MessageBox.Show("Values error");
                        }
                        break;
                    case SelectedShape.Ellipse:
                        if(int.Parse(widthTxt.Text) > 0 && int.Parse(heightTxt.Text) > 0 && (int.Parse(ticknessTxt.Text) > 0))
                        {
                            Rendershape = new Ellipse() { Fill = Brushes.OrangeRed, Height = int.Parse(heightTxt.Text), Width = int.Parse(widthTxt.Text) };
                            Canvas.SetLeft(Rendershape, e.GetPosition(canvasArea).X);
                            Canvas.SetTop(Rendershape, e.GetPosition(canvasArea).Y);
                            canvasArea.Children.Add(Rendershape);


                        }
                        else
                        {
                            MessageBox.Show("Values error");
                        }
                        break;
                    case SelectedShape.Rectangle:
                        if (int.Parse(widthTxt.Text) > 0 && int.Parse(heightTxt.Text) > 0 && (int.Parse(ticknessTxt.Text) > 0))
                        {
                            Rendershape = new Rectangle() { Fill = Brushes.DarkSalmon, Height = int.Parse(heightTxt.Text), Width = int.Parse(widthTxt.Text) };
                            Canvas.SetLeft(Rendershape, e.GetPosition(canvasArea).X);
                            Canvas.SetTop(Rendershape, e.GetPosition(canvasArea).Y);
                            canvasArea.Children.Add(Rendershape);

                        }
                        else
                        {
                            MessageBox.Show("Values error");
                        }
                        break;
                    default:
                        return;
                }
            }
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //-------------------------PRZESUWANIE-------------------------
            if (shiftBox.IsChecked == true && Keyboard.IsKeyDown(Key.F2))
                Canvas.SetTop(selectedElement, Canvas.GetTop(selectedElement) - 1);
            if (shiftBox.IsChecked == true && Keyboard.IsKeyDown(Key.F4))
                Canvas.SetTop(selectedElement, Canvas.GetTop(selectedElement) + 1);
            if (shiftBox.IsChecked == true && Keyboard.IsKeyDown(Key.F1))
                Canvas.SetLeft(selectedElement, Canvas.GetLeft(selectedElement) - 1);
            if (shiftBox.IsChecked == true && Keyboard.IsKeyDown(Key.F3))
                Canvas.SetLeft(selectedElement, Canvas.GetLeft(selectedElement) + 1);

            //-------------------------ROTACJA-------------------------
            if (rotationBox.IsChecked == true && Keyboard.IsKeyDown(Key.F1))
            {
                stopnie++;
                rotation = new RotateTransform(stopnie, x1, y1);
                transformation.Children.RemoveAt(0);
                transformation.Children.Insert(0, rotation);
                selectedElement.RenderTransform = transformation;
            }
        }
    }
}
