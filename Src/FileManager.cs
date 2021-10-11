using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tetris.Src
{
    public class FileManager<T> where T : new()
    {
        private readonly string fullPath;

        public FileManager(string fileName)
        {
            fullPath = Path.GetFullPath(fileName);
        }

        public T LoadData()
        {
            T data;

            FileStream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                data = (T)serializer.Deserialize(stream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.Close();
            }

            return data;
        }

        public void SaveData(T data)
        {
            FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.SetLength(stream.Position);
                stream.Close();
            }
        }
    }
}
