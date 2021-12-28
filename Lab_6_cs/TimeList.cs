using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Lab_6_cs
{
    [Serializable]
    public class TimeList
    {
        //закрытое поле типа List<TimeItem> - список объектов типа TimeItem
        private List<TimeItem> list = new List<TimeItem>();

        //метод, который добавляет новый объект TimeItem к списку
        public void Add(TimeItem ob)
        {
            list.Add(ob);
        }

        //метод для восстановления списка из файла 
        public void Load(string filename)
        {
            FileStream f = new FileStream(filename, FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (f.Length != 0)
                    list = (List<TimeItem>)bf.Deserialize(f);
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось загрузить файл! ");
                Console.WriteLine(e);
            }
            finally
            {
                f.Close();
            }
        }

        //метод для сохранения списка List<TimeItem> в файле 
        public void Save(string filename)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream f = new FileStream(filename, FileMode.Open);
                bf.Serialize(f, list);
                f.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось загрузить файл! ");
                Console.WriteLine(e);
            }
        }

        //перегруженная версия виртуального метода ToString
        public override string ToString()
        {
            string temp = "Размерность\tЧисло повторов\tВремя C#\tВремя С++\tРазность\n";
            int count = 1;
            foreach (TimeItem ob in list)
            {
                temp += count + ". " + ob + "\n";
                count++;
            }
            return temp;
        }
    }
}
