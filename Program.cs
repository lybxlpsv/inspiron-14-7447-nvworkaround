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
            //table 1084-1076-1063-1010-997-984-971
            public bool stahp = false;
            public int throttletemp_0 = 1;
            public int nothrottlestate = 1076;
            public int throttletemp_1 = 55;
            public int throttlestate_1 = 1075;
            public int throttletemp_2 = 57;
            public int throttlestate_2 = 1063;
            public int throttletemp_3 = 59;
            public int throttlestate_3 = 1010;
            public int throttletemp_4 = 60;
            public int throttlestate_4 = 997;
            public int throttletemp_5 = 61;
            public int throttlestate_5 = 984;
            public int memorystate = 1160;
            public int memorystate_temp1 = 55;
            public int memorystate_state1 = 1155;
            public int memorystate_temp2 = 60;
            public int memorystate_state2 = 1150;
            public int proposedstate = 960;
            public int proposedmemstate = 800;
            public int lastproposedstate = 960;
            public bool clockhaschanged = true;
            public bool lastgpuusage = false;
            public bool gpuusage = false;
            public int temp = 0;
            public int usage = 0;
            public void check()
            {
                while (stahp == false)
                {
                    try
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        GpuzWrapper gpuz = new GpuzWrapper();
                        gpuz.Open();
                        temp = (int)Math.Round(gpuz.SensorValue(2));
                        usage = (int)Math.Round(gpuz.SensorValue(3));
                        Console.WriteLine("== lybAdaptiveOC ==\nfor DELL GM107-A, SK Hynix, Target Temps 53-57\n");
                        Console.WriteLine("Factory GPU Clock" + ": 876 Mhz");
                        Console.WriteLine(gpuz.SensorName(0) + ": " + gpuz.SensorValue(0) + " " + gpuz.SensorUnit(0));
                        Console.WriteLine(gpuz.SensorName(2) + ": " + gpuz.SensorValue(2) + " " + gpuz.SensorUnit(2));
                        Console.WriteLine(gpuz.SensorName(3) + ": " + gpuz.SensorValue(3) + " " + gpuz.SensorUnit(3));
                        Console.WriteLine(gpuz.SensorName(10) + ": " + gpuz.SensorValue(10) + " " + gpuz.SensorUnit(10));
                        if ((temp >= throttletemp_0) && (temp < throttletemp_1)) { proposedstate = nothrottlestate; Console.WriteLine("OC STATE 5"); }
                        if ((temp >= throttletemp_1) && (temp < throttletemp_2)) { proposedstate = throttlestate_1; Console.WriteLine("OC STATE 4"); }
                        if ((temp >= throttletemp_2) && (temp < throttletemp_3)) { proposedstate = throttlestate_2; Console.WriteLine("OC STATE 3"); }
                        if ((temp >= throttletemp_3) && (temp < throttletemp_4)) { proposedstate = throttlestate_3; Console.WriteLine("OC STATE 2"); }
                        if ((temp >= throttletemp_4) && (temp < throttletemp_5)) { proposedstate = throttlestate_4; Console.WriteLine("OC STATE 1"); }
                        if (temp >= throttletemp_5) { proposedstate = throttlestate_5; Console.WriteLine("OC STATE 0"); }

                        if ((temp >= throttletemp_0) && (temp < memorystate_temp1)) { proposedmemstate = memorystate; Console.WriteLine("MEM OC STATE 2"); }
                        if ((temp >= memorystate_temp1) && (temp < memorystate_temp2)) { proposedmemstate = memorystate_state1; Console.WriteLine("MEM OC STATE 1"); }
                        if (temp >= memorystate_temp2) { proposedmemstate = memorystate_state2; Console.WriteLine("MEM OC STATE 0"); }

                        if (proposedstate != lastproposedstate) clockhaschanged = false;

                        if ((clockhaschanged == false) && ((int)gpuz.SensorValue(0) != 0))
                        {
                            var p = new Process();
                            p.StartInfo.FileName = @"C:\NVInspector\nvidiaInspector.exe";
                            //TODO : PATH CONFIGURATION
                            p.StartInfo.Arguments = "-setPstateLimit:0,5 -setGPUClock:0,1," + proposedstate + " -setMemoryClock:0,1," + proposedmemstate;
                            p.Start();
                            clockhaschanged = true;
                            lastproposedstate = proposedstate;
                        }

                        if (usage > 2) gpuusage = true; else gpuusage = false;

                        //DELL FAN CONTROL
                        if (lastgpuusage != gpuusage)
                        {
                            if (gpuusage == false)
                            {
                                
                            }
                            else
                            {
                                var d = new Process();
                                d.StartInfo.FileName = @"C:\Users\lybxl\Downloads\Compressed\RwPortableX64V1.6.9\Win64\Portable\Rw.exe";
                                //TODO : PATH CONFIGURATION
                                d.StartInfo.Arguments = "\"/command=WEC 96 68;RwExit;\"";
                                d.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                d.StartInfo.UseShellExecute = false;
                                d.StartInfo.RedirectStandardOutput = true;
                                d.StartInfo.RedirectStandardError = true;
                                //d.Start();

                            }
                            lastgpuusage = gpuusage;
                        }

                        Console.WriteLine("Target Core Clock   = " + proposedstate + "Mhz, OC +" + (proposedstate - 876) + "Mhz, " + (int)(proposedstate * 640 * 2 / 1000) + " Gflops");
                        Console.WriteLine("Target Memory Clock = " + proposedmemstate + "Mhz" + ", OC +" + (proposedmemstate - 800) + "Mhz" + ", " + (proposedmemstate * 128 * 2 / 8) + " Mb/s");
                        Console.WriteLine("ALT+F4 to EXIT");
                        gpuz.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                        gpuz.Close();
                    }
                    
                    Thread.Sleep(500);
                }
            }

        }
        static void Main(string[] args)
        {
            var c = new Process();
            c.StartInfo.FileName = @"C:\Users\Laptop\Downloads\RwPortableX64V1.6.9\Win64\Portable\Rw.exe";
            //TODO : PATH CONFIGURATION
            c.StartInfo.Arguments = "\"/command=WEC 96 68;RwExit;\"";
            c.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            c.StartInfo.UseShellExecute = false;
            c.StartInfo.RedirectStandardOutput = true;
            c.StartInfo.RedirectStandardError = true;
            c.Start();
            //TODO : RUN AS ADMIN CHECK
            //TODO : READ AND WRITE CONFIGURATION
            Alpha alp = new Alpha();
            var u = new Process();
            u.StartInfo.FileName = @"C:\NVInspector\nvidiaInspector.exe";
            //TODO : PATH CONFIGURATION
            u.StartInfo.Arguments = "-setPstateLimit:0,0";
            u.Start();
            Thread.Sleep(500);
            var n = new Process();
            n.StartInfo.FileName = @"C:\NVInspector\nvidiaInspector.exe";
            //TODO : PATH CONFIGURATION
            n.StartInfo.Arguments = "-setBaseClockOffset:0,0,135 -setMemoryClockOffset:0,0,260";
            n.Start();
            Thread.Sleep(500);
            var p = new Process();
            p.StartInfo.FileName = @"C:\NVInspector\nvidiaInspector.exe";
            //TODO : PATH CONFIGURATION
            p.StartInfo.Arguments = "-setPstateLimit:0,5 -setMemoryClock:0,1," + alp.memorystate;
            p.Start();
            Thread oThread = new Thread(new ThreadStart(alp.check));
            oThread.Start();
            Console.ReadLine();
        }
    }
}
