using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Settings;

namespace WhatsAppApi.Account
{
    public class WhatsContactsManager
    {
        private Dictionary<string, WhatsContact> contactsList;

        public WhatsContactsManager()
        {
            this.contactsList = new Dictionary<string, WhatsContact>();
        }

        public List<WhatsContact> GetAllContacts()
        {
            return contactsList.Values.ToList();
        }

        public List<WhatsContact> GetPersonContacts()
        {
            return contactsList.Values.Where(c => !c.IsGroup).ToList();
        }

        public List<WhatsContact> GetGroupContacts()
        {
            return contactsList.Values.Where(c => c.IsGroup).ToList();
        }
        
        //public void AddUser(User user)
        //{
        //    //if(user == null || user.)
        //    //if(this.userList.ContainsKey())
        //}

        public WhatsContact CreateUserContact(string jid, string nickname = "")
        {
            if (this.contactsList.ContainsKey(jid))
                return this.contactsList[jid];

            string server = WhatsConstants.WhatsAppServer;
            if (jid.Contains("-"))
                server = WhatsConstants.WhatsGroupChat;

            var tmpUser = new WhatsContact(jid, server, nickname);
            this.contactsList.Add(jid, tmpUser);
            return tmpUser;
        }
    }
}
