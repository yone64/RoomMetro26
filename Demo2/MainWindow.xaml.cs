using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
using Codeplex.Reactive;

namespace Demo2
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var down = Canvas.MouseDownAsObservable().Do(_ => Canvas.CaptureMouse()).Select(e => e.GetPosition(Canvas));
            var move = Canvas.MouseMoveAsObservable().Select(e => e.GetPosition(Canvas));
            var up = Canvas.MouseUpAsObservable().Do(_ => Canvas.ReleaseMouseCapture());

            this.Points = down
                .SelectMany(_ => move, (s, e) => new { s, e })
                .Select(a => new PointCollection(new[] { a.s, new Point(a.s.X, a.e.Y), a.e, new Point(a.e.X, a.s.Y) }))
                .TakeUntil(up)
                .Repeat()
                .ToReactiveProperty();
                        
            this.DataContext = this;
        }

        public ReactiveProperty<PointCollection> Points { get; private set; }
    }


    #region 拡張メソッド
    public static class FrameworkElementEx
    {
        public static IObservable<MouseButtonEventArgs> MouseDownAsObservable(this FrameworkElement source)
        {
            return Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => source.MouseDown += h,
                h => source.MouseDown -= h);
        }

        public static IObservable<MouseEventArgs> MouseMoveAsObservable(this FrameworkElement source)
        {
            return Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                h => (s, e) => h(e),
                h => source.MouseMove += h,
                h => source.MouseMove -= h);
        }

        public static IObservable<MouseButtonEventArgs> MouseUpAsObservable(this FrameworkElement source)
        {
            return Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => source.MouseUp += h,
                h => source.MouseUp -= h);
        }
    }
    #endregion
}
