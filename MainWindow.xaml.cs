using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TaskExample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        public MainWindow() {
            this.InitializeComponent();

            #region ignore this
            //Action<int, Rectangle, int> walk = (stepSize, racer, Length) => {
            //    double startPosition = Canvas.GetLeft(racer);
            //    for (int i = 0; i < Length; i += stepSize) {
            //        //Clamp(i + stepSize, 0, raceLength)
            //        Canvas.SetLeft(racer,
            //                       (double) Clamp((int) Canvas.GetLeft(racer) + stepSize,
            //                                      (int) startPosition,
            //                                      Length));
            //    }
            //};



            //Task<int> racerOneTask = new Task<int>(() => { return 10; });
            //racerOneTask.ContinueWith((previousTask) => {
            //    if (Canvas.GetLeft(this.RacerOne) < raceLength) {
            //        Canvas.SetLeft(this.RacerOne,
            //                       (double) Clamp((int) Canvas.GetLeft(this.RacerOne) + previousTask.Result,
            //                                      (int) Canvas.GetLeft(this.RacerOne),
            //                                      raceLength));
            //        //Thread.Sleep(10000);
            //    }
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            //Task<int> racerTwoTask = new Task<int>(() => { return 20; });
            //racerTwoTask.ContinueWith((previousTask) => {
            //    if (Canvas.GetLeft(this.RacerTwo) < raceLength) {
            //        Canvas.SetLeft(this.RacerTwo,
            //                       (double) Clamp((int) Canvas.GetLeft(this.RacerTwo) + previousTask.Result,
            //                                      (int) Canvas.GetLeft(this.RacerTwo),
            //                                      raceLength));
            //        //Thread.Sleep(1000);
            //    }
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            //racerOneTask.Start();
            //racerTwoTask.Start();
            #endregion
            #region
            /*
            int raceLength = 500;

            Task one = new Task(() => Application.Current.Dispatcher.Invoke(() => {
                while (Canvas.GetLeft(this.RacerOne) < raceLength) {
                    Canvas.SetLeft(this.RacerOne,
                                   (double) Clamp((int) Canvas.GetLeft(this.RacerOne) + 15,
                                                  (int) Canvas.GetLeft(this.RacerOne),
                                                  raceLength));

                    // Update UI
                    Application.Current.Dispatcher.Invoke(delegate { }, System.Windows.Threading.DispatcherPriority.Render);
                    Thread.Sleep(10);
                }
            }));

            Task two = new Task(() => Application.Current.Dispatcher.Invoke(() => {
                while (Canvas.GetLeft(this.RacerTwo) < raceLength) {
                    Canvas.SetLeft(this.RacerTwo,
                                   (double) Clamp((int) Canvas.GetLeft(this.RacerTwo) + 15,
                                                  (int) Canvas.GetLeft(this.RacerTwo),
                                                  raceLength));

                    // Update UI
                    Application.Current.Dispatcher.Invoke(delegate { }, System.Windows.Threading.DispatcherPriority.Render);
                    Thread.Sleep(10);
                }
            }));

            one.Start();
            two.Start();
            */
            #endregion


            this.StartRunners();

        }

        private async Task StartRunners() {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            List<Task> tasks = new List<Task>() {
                RunnerOne(tokenSource), RunnerTwo(tokenSource),
            };

            await Task.WhenAll(tasks);
            MyCanvas.Background = Brushes.DarkSlateBlue;
        }

        private Task RunnerOne(CancellationTokenSource tokenSource, int raceLength = 500, int step = 10) {
            return Task.Run(() => Application.Current.Dispatcher.Invoke(async () => {
                int delay = MyRandom.Next(10, 101);
                while (Canvas.GetLeft(this.RacerOne) < raceLength) {
                    if (tokenSource.Token.IsCancellationRequested) {
                        break;
                    }
                    Canvas.SetLeft(this.RacerOne,
                                   (double) Clamp((int) Canvas.GetLeft(this.RacerOne) + step,
                                                  (int) Canvas.GetLeft(this.RacerOne),
                                                  raceLength));

                    // Update UI
                    Application.Current.Dispatcher.Invoke(delegate { }, System.Windows.Threading.DispatcherPriority.Render);
                    await Task.Delay(delay);
                }
                tokenSource.Cancel();
            }));
        }

        private Task RunnerTwo(CancellationTokenSource tokenSource, int raceLength = 500, int step = 10) {
            return Task.Run(() => Application.Current.Dispatcher.Invoke(async () => {
                int delay = MyRandom.Next(10, 101);
                while (Canvas.GetLeft(this.RacerTwo) < raceLength) {
                    if (tokenSource.Token.IsCancellationRequested) {
                        break;
                    }
                    Canvas.SetLeft(this.RacerTwo,
                                   (double) Clamp((int) Canvas.GetLeft(this.RacerTwo) + step,
                                                  (int) Canvas.GetLeft(this.RacerTwo),
                                                  raceLength));

                    // Update UI
                    Application.Current.Dispatcher.Invoke(delegate { }, System.Windows.Threading.DispatcherPriority.Render);
                    await Task.Delay(delay);
                }
                tokenSource.Cancel();
            }));
        }

        public static Random MyRandom = new Random();

        public static int Clamp(int value, int min, int max) {
            return (value < min) ? min : ((value > max) ? max : value);
        }
    }

}
