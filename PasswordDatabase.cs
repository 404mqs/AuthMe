using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRewards
{
    public class PasswordDatabase
    {
        private DataStorage<List<Passwords>> DataStorage { get; set; }
        public List<Passwords> Data { get; private set; }
        public PasswordDatabase()
        {
            DataStorage = new DataStorage<List<Passwords>>(MQSPlugin.Instance.Directory, "Passwords.json");
        }
        public void Reload()
        {
            Data = DataStorage.Read();
            if (Data == null)
            {
                Data = new List<Passwords>();
                DataStorage.Save(Data);
            }
        }

        public void AddPassword(Passwords password)
        {
            Data.Add(password);
            DataStorage.Save(Data);
        }

        public bool RemovePassword(string name)
        {
            var flag = Data.RemoveAll(x => x.Name.Equals(name));
            if (flag > 0)
            {
                DataStorage.Save(Data);
                return true;
            }
            return false;
        }

        public void RemoveAll()
        {
            Data.Clear();
            DataStorage.Save(Data);
        }
    }
}
