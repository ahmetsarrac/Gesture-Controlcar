using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leap;
using System.IO.Ports;
using System.Media;

namespace TEZ
{
    public partial class Form1 : Form,ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;
        public Form1()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\ahmet\Desktop\TEZ\TEZ\obj\Debug\2.wav");
            simpleSound.Play();

            InitializeComponent();
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
            foreach (string portlar in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(portlar);
            }
           
            groupBox2.Enabled = false;
       
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 115200;
                serialPort1.Open();

                
            }
            catch (Exception)
            {
            }

            if (serialPort1.IsOpen)
            {
               
                groupBox2.Enabled = true;
               

            }

        }
        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                       
                        break;
                    case "onConnect":
                        break;
                    case "onFrame":
                        detectGesture(this.controller.Frame());
                        detectHandPosition(this.controller.Frame());
                  
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }
        public void detectGesture(Leap.Frame frame)
        {
            GestureList gestures = frame.Gestures(); 
        }
        public void detectHandPosition(Leap.Frame frame)
        {

            HandList allHands = frame.Hands;
            foreach(Hand hand in allHands)
            {
                
                float pitch = hand.Direction.Pitch;
                float yaw = hand.Direction.Yaw;
                float roll = hand.Direction.Roll;

                double degPitch = pitch * (180 / Math.PI);
                double degYaw = yaw * (180 / Math.PI);
                double degRoll = roll * (180 / Math.PI);

                int intPitch = (int)degPitch;
                int intYaw = (int)degYaw;
                int intRoll = (int)degRoll;

                float grab = hand.GrabStrength;

                textBox1.Text = intPitch.ToString();
                textBox2.Text = intYaw.ToString();
                textBox3.Text = intRoll.ToString();
                textBox4.Text = grab.ToString();

                if (groupBox2.Enabled == true)
                {
                    SoundPlayer player = new SoundPlayer(@"C:\Users\ahmet\Desktop\TEZ\TEZ\obj\Debug\2.wav");
                    player.Play();
                    
                    if (intPitch < -20)
                    {
                        serialPort1.Write("F");
                    }
                    else if (intPitch > 30)
                    {
                        serialPort1.Write("B");
                      
                    }
                    else if (intYaw < -20)
                    {
                        serialPort1.Write("M");
                    }
                    else if (intYaw > 20)
                    {
                        serialPort1.Write("N");
                    }
                    else 
                        serialPort1.Write("S");


                }
            }

        }

    }

    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            this.eventDelegate = delegateObject;
        }
        public override void OnInit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onInit");
        }
        public override void OnConnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onConnect");
        }
        public override void OnFrame(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onFrame");
        }
        public override void OnExit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onExit");
        }
        public override void OnDisconnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDisconnect");
        }
    }
}

