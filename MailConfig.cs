using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TSApi.Mail
{
    public class MailConfig
    {
        public string smtpServer { get; set; }
        public string mailUsername { get; set; }
        public string mailPassword { get; set; }
        public string mailFromAddress { get; set; }
        public string mailFromName { get; set; }

        public bool IsEnableSSL;
        public int securePort;


        private bool _IsError;
        private string _ErrorMessage;
        public bool IsError { get { return _IsError; } }
        public string ErrorMessage { get { return _ErrorMessage; } }

        public MailConfig()
        {
            _IsError = false;
            _ErrorMessage = string.Empty;

            try
            {
                smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                if (string.IsNullOrEmpty(smtpServer))
                {
                    _ErrorMessage = "SMTPServer not set in app.config.  Process aborted.";
                    _IsError = true;
                    return;
                }


                mailUsername = ConfigurationManager.AppSettings["MailUsername"];
                if (string.IsNullOrEmpty(mailUsername))
                {
                    _ErrorMessage = "MailUsername not set in app.config.  Process aborted.";
                    _IsError = true;
                    return;
                }


                mailPassword = ConfigurationManager.AppSettings["MailPassword"];
                if (string.IsNullOrEmpty(mailPassword))
                {
                    _ErrorMessage = "MailPassword not set in app.config.  Process aborted.";
                    _IsError = true;
                    return;
                }

                mailFromAddress = ConfigurationManager.AppSettings["MailFromAddress"];
                if (string.IsNullOrEmpty(mailFromAddress))
                {
                    _ErrorMessage = "MailFromAddress not set in app.config.  Process aborted.";
                    _IsError = true;
                    return;

                }
                mailFromName = ConfigurationManager.AppSettings["MailFromName"];
                if (string.IsNullOrEmpty(mailFromName))
                {
                    _ErrorMessage = "MailFromName not set in app.config.  Process aborted.";
                    _IsError = true;
                    return;
                }

                IsEnableSSL = ConfigurationManager.AppSettings["MailEnableSSL"] == "true";

                if (IsEnableSSL)
                {
                    string s = ConfigurationManager.AppSettings["MailPort"];

                    if (!int.TryParse(s, out securePort))
                    {
                        _ErrorMessage = "SecurePort not set in app.config.  Process aborted.";
                        _IsError = true;
                        return;
                    }

                }
            }
            catch (Exception ee)
            {
                _IsError = true;
                _ErrorMessage = "MailConfig Exception: Message " + ee.Message;
            }

        }
    }
}
