using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeRewards
{
    public class PasswordServices : MonoBehaviour
    {
        public Dictionary<string, List<Passwords>> PasswordList { get; set; }

        public PasswordDatabase database => MQSPlugin.Instance.PasswordDatabase;

        void Awake()
        {
            PasswordList = new Dictionary<string, List<Passwords>>();
        }

        void Start()
        {

        }

        void OnDestroy()
        {

        }

        public bool RightPassword(string password)
        {
            var pass = database.Data.FirstOrDefault(x => x.Password.Equals(password));

            if (pass == null)
            {
                return false;
            }
            return true;
        }

        public bool IsRegistered(string name)
        {
            var username = database.Data.FirstOrDefault(x => x.Name.Equals(name));

            if (username == null)
            {
                return false;
            }
            return true;
        }

        public void RegisterPassword(string name, string id64, string password)
        {
            var userpassword = new Passwords
            {
                Name = name,
                ID = id64,
                Password = password
            };

            database.AddPassword(userpassword);
        }

        public void ResetPassword(string name, string id64, string password)
        {

            database.RemovePassword(name);
        }

        public void AdminResetPassword(string name)
        {
            database.RemovePassword(name);
        }

        public void ClearData()
        {
            database.RemoveAll();
        }
    }
}
