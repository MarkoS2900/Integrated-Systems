using System;

namespace Cinema.Domain
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPass { get; set; }
        public int SMTPPort { get; set; }
        public bool EnableSSL { get; set; }
        public string EmailDisplayName { get; set; }
        public string SenderName { get; set; }

        public EmailSettings() { }

        public EmailSettings(string sMTPServer, string sMTPUsername, string sMPRPass, int sMTPPort)
        {
            SMTPServer = sMTPServer;
            SMTPUsername = sMTPUsername;
            SMTPPass = sMPRPass;
            SMTPPort = sMTPPort;
        }
    }
}
