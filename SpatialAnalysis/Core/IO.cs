using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpatialAnalysis.Core
{
    class IO
    {
        /// <summary>
        /// 往文件中写数据
        /// </summary>
        /// <param name="recordDetail"></param>
        /// <param name="warningRecord"></param>
        /// <param name="wrType"></param>
        public static void WriteData(Object obj, string baseDir, string fileName)
        {

            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                //创建文件夹
                byte[] pReadByte = new byte[3000];
                string dataDir = baseDir;
                if (!Directory.Exists(dataDir))// 目录不存在,新建目录
                {
                    Directory.CreateDirectory(dataDir);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    IFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, obj);
                    pReadByte = ms.GetBuffer();
                }
                //将二进制写入文件
                string str = string.Empty;
                for (int i = 0; i < pReadByte.Length; i++)
                {
                    str += pReadByte[i].ToString("X2");
                }
                fs = new FileStream(dataDir + "\\" + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, System.IO.SeekOrigin.End);
                //文件节流点
                sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.WriteLine(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                return;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
        }

        public static Object ReadDataToMemory(string baseDir, string fileName)
        {
            Object obj = new object();
            FileStream fs = null;
            StreamReader read = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                //获取用户记录信息到内存字典中
                string dataDir = baseDir;
                fs = new FileStream(dataDir + "\\" + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                read = new StreamReader(fs);
                //循环读取每一行
                string strReadline = string.Empty;
                while ((strReadline = read.ReadLine()) != null)
                {
                    //对每一行数据做反序列化处理
                    if (strReadline.Length % 2 != 0)
                    {
                        strReadline = "0" + strReadline;
                    }
                    byte[] binReadline = new byte[strReadline.Length / 2];
                    for (int i = 0; i < binReadline.Length; i++)
                    {
                        string b = strReadline.Substring(i * 2, 2);
                        binReadline[i] = Convert.ToByte(b, 16);
                    }
                    using (MemoryStream ms = new MemoryStream(binReadline))
                    {
                        IFormatter iFormatter = new BinaryFormatter();
                        obj = (Object)iFormatter.Deserialize(ms);
                    }
                }               
                return obj;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
                if (read != null)
                {
                    read.Close();
                    read = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }  
                return null;
            }
            finally
            {                
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }                
            }
        }
    }
}
