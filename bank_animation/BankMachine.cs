using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace bank_animation
{
    class BankMachine
    {
        Client wClient;// = new Client();
        Money[] _cash;
        int _allMoney;
        bool _result;
        DispatcherTimer workTime = new DispatcherTimer();

        public BankMachine()
        {
            _allMoney = 0;
            Cash = new Money[6];
            Random rand = new Random();
            Cash[0] = new Money(10000, 0);
            Cash[1] = new Money(5000, 0);
            Cash[2] = new Money(1000, 0);
            Cash[3] = new Money(500, 0);
            Cash[4] = new Money(100, 0);
            Cash[5] = new Money(50, 0);
            for (int i = 0; i < Cash.Length; i++)
            {
                Cash[i].Count = rand.Next(1, 10000 / Cash[i].Cost + 3);
                _allMoney += Cash[i].Count;
            }
        }

        public delegate void ResultRequest(bool result, Client client);
        public event ResultRequest SentResult;

        public void Work(Client client)
        {
            wClient = client;
            _result = GetRequest(client.RequestSum);
            workTime.Interval = TimeSpan.FromSeconds(2);
            workTime.Tick += workTime_Tick;
            SentResult += client.GoAway;
            if (Result)
            {
                workTime.Start();
            }
            else
            {
                while (client.CountOfRequest < Consts.clientRequest && !Result)
                {
                    _result = GetRequest(client.ChangeRequest());
                    workTime.Start();
                }
            }
        }

        private void workTime_Tick(object sender, EventArgs e)
        {
            workTime.Stop();
            if (wClient.CountOfRequest < Consts.clientRequest || Result)
            {
                SentResult(Result, wClient);
                MessageBox.Show(Result.ToString() + " " + wClient.RequestSum.ToString());
            }
        }

        public bool GetRequest(int sum)
        {
            if (sum < Consts.maxSum && sum > Consts.minSum)
            {
                int[] beforCount = { Cash[0].Count, Cash[1].Count, Cash[2].Count, Cash[3].Count, Cash[4].Count, Cash[5].Count };

                for (int i = 0; i < Cash.Length; i++)
                {
                    while (sum > 0 && Cash[i].Count != 0)
                    {
                        if (sum - Cash[i].Cost >= 0)
                        {
                            sum -= Cash[i].Cost;
                            Cash[i].Count--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (sum == 0)
                    {
                        return true;
                    }
                }
                for (int i = 0; i < Cash.Length; i++)
                {
                    Cash[i].Count = beforCount[i];
                }
            }
            return false;
        }

        public Money[] Cash
        {
            get { return _cash; }
            set { _cash = value; }
        }

        public bool Result
        {
            get { return _result; }
        }

        public int AllMoney
        {
            get { return _allMoney; }
        }
    }
}
