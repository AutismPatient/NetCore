using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreInit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await WriteFile("Baidu.html");
            //ReadFile("Baidu.html");
            var files = GetFiles("D:\\work-html");
            if(files.Length > 0)
            {
                foreach(var f in files)
                {
                    Console.WriteLine(f.Name);
                }
            }
            Console.ReadKey();
        }
        /// <summary>
        /// 创建并写入文件
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static async Task WriteFile(string fn)
        {
            FileStream stream = new FileStream(fn, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            HttpClient client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(20)
            };
            var resp = await client.GetAsync("https://www.baidu.com/");

            if (resp.IsSuccessStatusCode)
            {
                writer.Write(await resp.Content.ReadAsStringAsync());
                writer.Close();
            }
            await stream.DisposeAsync();
            stream.Close();
            Console.WriteLine("file is white ok");
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        public static void ReadFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    fs.Lock(fs.Position,fs.Length);
                    BinaryReader reader = new BinaryReader(fs);
                    Console.WriteLine("{0} --> {1}", fs.Name, reader.ReadString());
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 读取某个文件夹的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string path)
        {
            if (string.IsNullOrEmpty(path)) return default;
            var df = new DirectoryInfo(path);
            if (!df.Exists) Console.WriteLine("文件夹路径不存在！");
            return df.GetFiles("*",searchOption:SearchOption.TopDirectoryOnly);
        }
    }
}
