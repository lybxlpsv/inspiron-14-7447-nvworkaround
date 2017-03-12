using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GpuzDemo
{
    class Program
    {
        public class Alpha
        {
            public bool stahp = false;
            public int throttletemp_0 = 1;
            public int nothrottlestate = 100;
            public int throttletemp_1 = 60;
            public int throttlestate_1 = 80;
            public int throttletemp_2 = 61;
            public int throttlestate_2 = 50;
            public int throttletemp_3 = 62;
            public int throttlestate_3 = 45;
            public int throttletemp_4 = 63;
            public int throttlestate_4 = 30;
            public int throttletemp_5 = 64;
            public int throttlestate_5 = 21;
            public int throttletemp_6 = 65;
            public int throttlestate_6 = 0;
            public int throttletemp_7 = 66;
            public int throttlestate_7 = -20;
            public int throttletemp_8 = 67;
            public int throttlestate_8 = -40;
            public int throttletemp_9 = 68;
            public int throttlestate_9 = -50;
            public int throttletemp_10 = 70;
            public int throttlestate_10 = -100;
            public int proposedstate = 0;
            public int lastproposedstate = 0;
            public bool clockhaschanged = true;
            public int temp = 0;
            public void check()
            {
                while (stahp == false)
                {

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    GpuzWrapper gpuz = new GpuzWrapper();
                    gpuz.Open();
                    temp = (int)Math.Round(gpuz.SensorValue(2));
                    Console.WriteLine("Factory GPU Clock" + ": " + gpuz.DataValue(22) + "Mhz");
                    Console.WriteLine(gpuz.SensorName(0) + ": " + gpuz.SensorValue(0) + " " + gpuz.SensorUnit(0));
                    Console.WriteLine(gpuz.SensorName(2) + ": " + gpuz.SensorValue(2) + " " + gpuz.SensorUnit(2));
                    Console.WriteLine(gpuz.SensorName(3) + ": " + temp + " " + gpuz.SensorUnit(3));
                    Console.WriteLine(gpuz.SensorName(10) + ": " + gpuz.SensorValue(10) + " " + gpuz.SensorUnit(10));
                    if ((temp >= throttletemp_0) && (temp < throttletemp_1)) { proposedstate = nothrottlestate; Console.WriteLine("NO THROTTLING"); }
                    if ((temp >= throttletemp_1) && (temp < throttletemp_2)) { proposedstate = throttlestate_1; Console.WriteLine("THROTTLING STATE 1"); }
                    if ((temp >= throttletemp_2) && (temp < throttletemp_3)) { proposedstate = throttlestate_2; Console.WriteLine("THROTTLING STATE 2"); }
                    if ((temp >= throttletemp_3) && (temp < throttletemp_4)) { proposedstate = throttlestate_3; Console.WriteLine("THROTTLING STATE 3"); }
                    if ((temp >= throttletemp_4) && (temp < throttletemp_5)) { proposedstate = throttlestate_4; Console.WriteLine("THROTTLING STATE 4"); }
                    if ((temp >= throttletemp_5) && (temp < throttletemp_6)) { proposedstate = throttlestate_5; Console.WriteLine("THROTTLING STATE 5"); }
                    if ((temp >= throttletemp_6) && (temp < throttletemp_7)) { proposedstate = throttlestate_6; Console.WriteLine("THROTTLING STATE 6"); }
                    if ((temp >= throttletemp_7) && (temp < throttletemp_8)) { proposedstate = throttlestate_7; Console.WriteLine("THROTTLING STATE 7"); }
                    if ((temp >= throttletemp_8) && (temp < throttletemp_9)) { proposedstate = throttlestate_8; Console.WriteLine("THROTTLING STATE 8"); }
                    if ((temp >= throttletemp_9) && (temp < throttletemp_10)) { proposedstate = throttlestate_9; Console.WriteLine("THROTTLING STATE 9"); }
                    if (temp >= throttletemp_10) { proposedstate = throttlestate_10; Console.WriteLine("THROTTLING STATE 10 --HOT!--"); }

                    if (proposedstate != lastproposedstate) clockhaschanged = false;

                    if (clockhaschanged == false)
                    {
                        var p = new Process();
                        p.StartInfo.FileName = @"C:\Users\lybxl\Downloads\NV-Inspector-[Guru3D.com]\Guru3D.com\nvidiaInspector.exe";
                        p.StartInfo.Arguments = "-setPstateLimit:0,0 -setBaseClockOffset:0,0," + proposedstate;
                        p.Start();
                        clockhaschanged = true;
                        lastproposedstate = proposedstate;
                    }

                    Console.WriteLine("Proposed GPU Clock = " + (proposedstate + 1084) + "Mhz, OC +" + ((1084+proposedstate)-900) + "Mhz");
                    Console.WriteLine("Press ENTER to quit");
                    gpuz.Close();
                    Thread.Sleep(500);
                }
            }

        }

            static void Main(string[] args)
        {
            var p = new Process();
            Alpha alp = new Alpha();
            
            Thread oThread = new Thread(new ThreadStart(alp.check));
            oThread.Start();
            Console.ReadLine();
        }
    }
}
