using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace bank_animation
{
    class WorkProcess
    {
        BankMachine machine;
        Queue<Client> clients;
        Client deadMan;
        Canvas canvas;
        DispatcherTimer clientTimer = new DispatcherTimer();
        DispatcherTimer killTimer = new DispatcherTimer();
        Random rand = new Random();

        public WorkProcess(Canvas myCanvas)
        {
            machine = new BankMachine();
            clients = new Queue<Client>();
            canvas = myCanvas;
            clientTimer.Interval = TimeSpan.FromSeconds(rand.Next(5, 30));
            clientTimer.Tick += ClientTimer_Tick;
            clientTimer.Start();
        }

        public void StartWork(Client awayCient)
        {
            if (clients.Count > 0)
            {
                clients.Dequeue();
                if (clients.Count > 0)
                {
                    MoveQueue();
                    ClientRequest(clients.Peek());
                }
            }
        }

        private void Delete(Client client)
        {
            killTimer.Interval = TimeSpan.FromSeconds(2);
            killTimer.Tick += killTimer_Tick;
            deadMan = client;
            killTimer.Start();
        }

        private double CountDistination()
        {
            double distance = (Consts.clientCount - clients.Count + 1) * 70;
            return distance;
        }

        private void ClientTimer_Tick(object sender, EventArgs e)
        {
            Client client = new Client(400, 200);
            if (clients.Count < Consts.clientCount)
            {
                client.QueuePosition = clients.Count + 1; 
                clients.Enqueue(client);
                if (client.QueuePosition == 1)
                {
                    client.DeleteClient += Delete;
                    client.ReadyRequest += ClientRequest;
                }
                client.Animate(CountDistination());
                canvas.Children.Add(client);
            }
            else
            {
                client.NoPlace();
                canvas.Children.Add(client);
                //deadMan = client;
                Delete(client);
                //killTimer.Start();
            }
            clientTimer.Interval = TimeSpan.FromSeconds(rand.Next(5, 30));
        }

        private void MoveQueue()
        {
            int count = clients.Count;
            for (int i = 0; i < count; i++)
            {
                Client moveClient = clients.Dequeue();
                clients.Enqueue(moveClient);
                moveClient.QueuePosition--;
                moveClient.AnimStep(70);
                if (moveClient.QueuePosition == 1)
                {
                    moveClient.DeleteClient += Delete;
                    moveClient.ReadyRequest += ClientRequest;
                }
            }
        }

        //private void move_Tick(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void ClientRequest(Client client)
        {
            Client workClient = client;
            workClient.Request += machine.Work;
            workClient.ChangePosition += StartWork;
            workClient.SentRequest(null, null);
        }

        private void killTimer_Tick(object sender, EventArgs e)
        {
            killTimer.Stop();
            canvas.Children.Remove(deadMan);
            //deadMan = null;
        }
    }
}
