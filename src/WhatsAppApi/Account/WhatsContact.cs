using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Settings;

namespace WhatsAppApi.Account
{
    public class WhatsContact
    {
        private string serverUrl;
        public string Nickname { get; set; }
        public string Jid { get; private set; }

        public WhatsContact(string jid, string srvUrl, string nickname = "")
        {
            this.Jid = jid;
            this.Nickname = nickname;
            this.serverUrl = srvUrl;
        }

        public string GetFullJid()
        {
            return string.Format("{0}@{1}", this.Jid, this.serverUrl);
        }

        public bool IsGroup {
            get { return this.serverUrl.Equals(WhatsConstants.WhatsGroupChat, StringComparison.OrdinalIgnoreCase); }
        }

        internal void SetServerUrl(string srvUrl)
        {
            this.serverUrl = srvUrl;
        }

        public override string ToString()
        {
            return this.GetFullJid();
        }
    }
}
