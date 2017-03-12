using GpuzDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nvavoidthrottling
{
    class Program
    {
        public class Alpha
        {
            public bool stahp = false;
            public int throttletemp_0 = 1;
            public int nothrottlestate = 1084;
            public int throttletemp_1 = 53;
            public int throttlestate_1 = 1000;
            public int throttletemp_2 = 54;
            public int throttlestate_2 = 960;
            public int throttletemp_3 = 55;
            public int throttlestate_3 = 940;
            public int throttletemp_4 = 56;
            public int throttlestate_4 = 900;
            public int throttletemp_5 = 57;
            public int throttlestate_5 = 876;
            public int memorystate = 1149;
            public int proposedstate = 960;
            public int lastproposedstate = 960;
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
                    Console.WriteLine("Factory GPU Clock" + ": 876 Mhz");
                    Console.WriteLine(gpuz.SensorName(0) + ": " + gpuz.SensorValue(0) + " " + gpuz.SensorUnit(0));
                    Console.WriteLine(gpuz.SensorName(2) + ": " + gpuz.SensorValue(2) + " " + gpuz.SensorUnit(2));
                    Console.WriteLine(gpuz.SensorName(4) + ": " + gpuz.SensorValue(4) + " " + gpuz.SensorUnit(4));
                    Console.WriteLine(gpuz.SensorName(10) + ": " + gpuz.SensorValue(10) + " " + gpuz.SensorUnit(10));
                    if ((temp >= throttletemp_0) && (temp < throttletemp_1)) { proposedstate = nothrottlestate; Console.WriteLine("OC STATE 5"); }
                    if ((temp >= throttletemp_1) && (temp < throttletemp_2)) { proposedstate = throttlestate_1; Console.WriteLine("OC STATE 4"); }
                    if ((temp >= throttletemp_2) && (temp < throttletemp_3)) { proposedstate = throttlestate_2; Console.WriteLine("OC STATE 3"); }
                    if ((temp >= throttletemp_3) && (temp < throttletemp_4)) { proposedstate = throttlestate_3; Console.WriteLine("OC STATE 2"); }
                    if ((temp >= throttletemp_4) && (temp < throttletemp_5)) { proposedstate = throttlestate_4; Console.WriteLine("OC STATE 1"); }
                    if (temp >= throttletemp_5) { proposedstate = throttlestate_5; Console.WriteLine("OC STATE 0"); }

                    if (proposedstate != lastproposedstate) clockhaschanged = false;

                    if (clockhaschanged == false)
                    {
                        var p = new Process();
                        p.StartInfo.FileName = @"C:\Users\lybxl\Downloads\NV-Inspector-[Guru3D.com]\Guru3D.com\nvidiaInspector.exe";
                        //TODO : PATH CONFIGURATION
                        p.StartInfo.Arguments = "-setPstateLimit:0,5 -setGPUClock:0,1," + proposedstate;
                        p.Start();
                        clockhaschanged = true;
                        lastproposedstate = proposedstate;
                    }

                    Console.WriteLine("Target GPU Clock  = " + proposedstate + "Mhz, OC +" + (proposedstate-876) + "Mhz");
                    Console.WriteLine("Target GPU Memory = " + memorystate + "Mhz" + ", OC +" + (memorystate -800) + "Mhz");
                    Console.WriteLine("Press ENTER to quit");
                    gpuz.Close();
                    Thread.Sleep(500);
                }
            }

        }
        static void Main(string[] args)
        {
            //TODO : RUN AS ADMIN CHECK
            //TODO : READ AND WRITE CONFIGURATION
            Alpha alp = new Alpha();
            var p = new Process();
            p.StartInfo.FileName = @"C:\Users\lybxl\Downloads\NV-Inspector-[Guru3D.com]\Guru3D.com\nvidiaInspector.exe";
            //TODO : PATH CONFIGURATION
            p.StartInfo.Arguments = "-setPstateLimit:0,5 -setMemoryClock:0,1," + alp.memorystate;
            p.Start();
            Thread oThread = new Thread(new ThreadStart(alp.check));
            oThread.Start();
            Console.ReadLine();
        }
    }
}
