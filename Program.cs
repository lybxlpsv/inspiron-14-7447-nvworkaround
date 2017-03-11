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
            public int throttletemp_1 = 55;
            public int throttlestate_1 = 1000;
            public int throttletemp_2 = 57;
            public int throttlestate_2 = 960;
            public int throttletemp_3 = 60;
            public int throttlestate_3 = 900;
            public int memorystate = 900;
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
                    Console.WriteLine("Factory GPU Clock" + ": " + gpuz.DataValue(22) + "Mhz");
                    Console.WriteLine(gpuz.SensorName(0) + ": " + gpuz.SensorValue(0) + " " + gpuz.SensorUnit(0));
                    Console.WriteLine(gpuz.SensorName(2) + ": " + gpuz.SensorValue(2) + " " + gpuz.SensorUnit(2));
                    Console.WriteLine(gpuz.SensorName(3) + ": " + temp + " " + gpuz.SensorUnit(3));
                    Console.WriteLine(gpuz.SensorName(10) + ": " + gpuz.SensorValue(10) + " " + gpuz.SensorUnit(10));
                    if (temp >= throttletemp_0) { proposedstate = nothrottlestate; Console.WriteLine("NO THROTTLING"); }
                    if ((temp >= throttletemp_1) && (temp < throttletemp_2)) { proposedstate = throttlestate_1; Console.WriteLine("THROTTLING STATE 1"); }
                    if ((temp >= throttletemp_2) && (temp < throttletemp_3)) { proposedstate = throttlestate_2; Console.WriteLine("THROTTLING STATE 2"); }
                    if (temp >= throttletemp_3) { proposedstate = throttlestate_3; Console.WriteLine("THROTTLING STATE 3"); }

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

                    Console.WriteLine("Proposed GPU Clock = " + proposedstate + "Mhz");
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
