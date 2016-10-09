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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace bank_animation
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : UserControl
    {
        int _requestSum;
        int _countOfRequest;
        int _queuePosition;
        Random rand = new Random();
        DispatcherTimer animate = new DispatcherTimer();
        DispatcherTimer animateAway = new DispatcherTimer();
        DispatcherTimer animColor = new DispatcherTimer();
        DispatcherTimer animateStep = new DispatcherTimer();
        double _distanation;

        public Client(int left, int top)
        {
            InitializeComponent();
            _distanation = 0;
            Margin = new Thickness(left, top, 0, 0);
            RequestSum = RoundRequest(rand.Next(Consts.minSum, Consts.maxSum + 1));
            CountOfRequest = 1;
        }

        public void Animate(double distance)
        {
            _distanation = distance;
            animate.Interval = TimeSpan.FromMilliseconds(50);
            animate.Tick += animate_Tick;
            animate.Start();
        }
        private void animate_Tick(object sender, EventArgs e)
        {
            if (_distanation == 0)
            {
                animate.Stop();
                if (QueuePosition == 1)
                {
                    SentReady(null, null);
                }
            }
            else
            {
                _distanation -= 2.5;
                Margin = new Thickness(Margin.Left - 2.5, Margin.Top, 0, 0);
            }
        }

        public void AnimStep(double distance)
        {
            _distanation += distance;
            animateStep.Interval = TimeSpan.FromMilliseconds(50);
            animateStep.Tick += animateStep_Tick;
            animate.Start();
        }
        private void animateStep_Tick(object sender, EventArgs e)
        {
            if (_distanation == 0)
            {
                animate.Stop();
                if (QueuePosition == 1)
                {
                    SentReady(null, null);
                }
            }
            else
            {
                _distanation -= 2.5;
                Margin = new Thickness(Margin.Left - 2.5, Margin.Top, 0, 0);
            }
        }

        public delegate void Delete(Client client);
        public event Delete DeleteClient;

        public delegate void Move(Client client);
        public event Move Request;
        public void SentRequest(object sender, EventArgs e)
        {
            Request(this);
        }

        public delegate void Ready(Client client);
        public event Ready ReadyRequest;
        public void SentReady(object sender, EventArgs e)
        {
            ReadyRequest(this);
        }

        public delegate void ChangePos(Client client);
        public event ChangePos ChangePosition;
        public void Change(object sender, EventArgs e)
        {
            ChangePosition(this);
        }

        //public delegate void Success();
        //public event Success RequestResult;

        //public delegate void Place();
        //public event Place PlaceInQueue;

        public void GoAway(bool result, Client client)
        {
            if (result)
            {
                client_png.Source = new BitmapImage(new Uri("C:/Users/EgorV_000.PROMETEUS/Documentation/Univer/Programm/С#-proj/bank_animation/bank_animation/Image/greenHuman.png"));
            }
            else
            {
                client_png.Source = new BitmapImage(new Uri("C:/Users/EgorV_000.PROMETEUS/Documentation/Univer/Programm/С#-proj/bank_animation/bank_animation/Image/redHuman.png"));
            }
            client._distanation = Consts.clientCount * 70;
            animateAway.Interval = TimeSpan.FromMilliseconds(50);
            animateAway.Tick += animateAway_Tick;
            animateAway.Start();
            Change(null, null);
        }
        private void animateAway_Tick(object sender, EventArgs e)
        {
            if (_distanation == 0)
            {
                animate.Stop();
                DeleteClient(this);
            }
            else
            {
                _distanation -= 2.5;
                Margin = new Thickness(Margin.Left + 2.5, Margin.Top, 0, 0);
            }
        }

        public void NoPlace()
        {
            client_png.Source = new BitmapImage(new Uri("C:/Users/EgorV_000.PROMETEUS/Documentation/Univer/Programm/С#-proj/bank_animation/bank_animation/Image/redHuman.png"));
            animColor.Interval = TimeSpan.FromSeconds(3);
            animColor.Tick += animColor_Tick;
            animColor.Start();
        }
        private void animColor_Tick(object sender, EventArgs e)
        {
            animColor.Stop();
        }



        public int ChangeRequest()
        {
            Random rand = new Random();
            int myReq = rand.Next(Consts.minSum, Consts.maxSum + 1);
            while (myReq == RequestSum)
            {
                myReq = RoundRequest(rand.Next(Consts.minSum, Consts.maxSum + 1));
            }
            CountOfRequest++;
            return RequestSum = myReq;
        }

        // Метод округления суммы до числа, делящегося на 50
        int RoundRequest(int sum)
        {
            int temp = sum / 1000;
            string final;
            if (temp > 0)
            {
                final = temp.ToString() + ((sum % 1000) / 100).ToString();
                temp = (sum % 1000) % 100;
                if (temp < 50)
                {
                    if (temp < 25)
                    {
                        return Convert.ToInt32(final) * 100;
                    }
                    else
                    {
                        return Convert.ToInt32(final) * 100 + 50;
                    }
                }
                else
                {
                    if (temp < 75)
                    {
                        return Convert.ToInt32(final) * 100 + 50;
                    }
                    else
                    {
                        return Convert.ToInt32(final) * 100 + 100;
                    }
                }
            }
            else
            {
                final = (sum / 100).ToString();
                temp = sum % 100;
                if (temp < 50)
                {
                    if (temp < 25)
                    {
                        return Convert.ToInt32(final) * 100;
                    }
                    else
                    {
                        return Convert.ToInt32(final) * 100 + 50;
                    }
                }
                else
                {
                    if (temp < 75)
                    {
                        return Convert.ToInt32(final) * 100 + 50;
                    }
                    else
                    {
                        return Convert.ToInt32(final) * 100 + 100;
                    }
                }
            }
        }

        public int RequestSum
        {
            get { return _requestSum; }
            set { _requestSum = value; }
        }

        public int CountOfRequest
        {
            get { return _countOfRequest; }
            set { _countOfRequest = value; }
        }

        public int QueuePosition
        {
            get { return _queuePosition; }
            set { _queuePosition = value; }
        }
    }
}
