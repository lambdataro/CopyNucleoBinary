using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace CopyNucleoBinary
{
    class Program
    {
        /// <summary>
        /// エントリーポイント
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    ForceCopyFile(FindLocalBinFile(), GetMpuPath());
                    Environment.Exit(0);
                }
                else if (args.Length == 1)
                {
                    if (File.Exists(args[0]) && Path.GetExtension(args[0]) == ".bin")
                    {
                        ForceCopyFile(args[0], GetMpuPath());
                        Environment.Exit(0);
                    }
                }

                throw new ApplicationException("コマンドライン引数が不正");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// ファイルをコピーする
        /// </summary>
        /// <param name="srcPath">コピー元</param>
        /// <param name="dstPath">コピー先</param>
        static void ForceCopyFile(string srcPath, string dstPath)
        {
            Console.WriteLine("\"{0}\" => \"{1}\"", Path.GetFileName(srcPath), dstPath);

            var info = new ProcessStartInfo("cp");
            info.Arguments = String.Format("\"{0}\" \"{1}\"", srcPath, dstPath);
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            using (var proc = new Process())
            {
                proc.StartInfo = info;
                proc.Start();
                proc.WaitForExit();
            }

            Console.WriteLine("転送完了");
        }

        /// <summary>
        /// カレントディレクトリ以下からバイナリファイルを探す
        /// </summary>
        /// <returns>バイナリファイルのパス</returns>
        static string FindLocalBinFile()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Directory.GetFiles(current, "*.bin", SearchOption.AllDirectories).FirstOrDefault();

            if (path == "")
            {
                throw new ApplicationException("バイナリファイルが見つからない");
            }

            return path;
        }

        /// <summary>
        /// 対応するMPUのディレクトリラベル名
        /// </summary>
        static HashSet<string> targetNames = new HashSet<string>
        {
            "NODE_F401RE",  // Nucleo F401RE
            "DIS_F746NG",   // STM32 F7 Discovery
        };

        /// <summary>
        /// 接続されているMPUを探す
        /// </summary>
        /// <returns>接続されているMPUのディレクトリパスを取得する</returns>
        static string GetMpuPath()
        {
            var path = DriveInfo.GetDrives().
                Where(drive => drive.IsReady && targetNames.Contains(drive.VolumeLabel)).
                Select(drive => drive.Name).
                FirstOrDefault();

            if (path == "")
            {
                throw new ApplicationException("MPUが接続されていません");
            }

            return path;
        }
    }
}
