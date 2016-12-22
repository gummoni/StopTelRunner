using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StopTelRunner
{
    class Program
    {
        static bool IsPower = true;
        static Task task;

        static void Main(string[] args)
        {
            task = Task.Run((Action)TaskMain);
            Console.WriteLine("exit = hit any key");
            Console.ReadKey();
            IsPower = false;
        }


        static void TaskMain()
        {
            while (IsPower)
            {
                var telRunner = "CompatTelRunner";
                var plist = GetProcessesByFileName(telRunner);
                if (null != plist)
                {
                    foreach (var proc in plist)
                    {
                        proc.Kill();
                        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Killed.");
                    }
                }
                Thread.Sleep(1000);
            }
        }


        public static Process[] GetProcessesByFileName(string searchFileName)
        {
            searchFileName = searchFileName.ToLower();
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            //すべてのプロセスを列挙する
            foreach (System.Diagnostics.Process p
                in System.Diagnostics.Process.GetProcesses())
            {
                string fileName;
                try
                {
                    //メインモジュールのパスを取得する
                    fileName = p.MainModule.FileName;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    //MainModuleの取得に失敗
                    fileName = "";
                }
                if (0 < fileName.Length)
                {
                    //ファイル名の部分を取得する
                    fileName = System.IO.Path.GetFileName(fileName);
                    //探しているファイル名と一致した時、コレクションに追加
                    if (searchFileName.Equals(fileName.ToLower()))
                    {
                        list.Add(p);
                    }
                }
            }

            //コレクションを配列にして返す
            return (System.Diagnostics.Process[])
                list.ToArray(typeof(System.Diagnostics.Process));
        }
    }
}
