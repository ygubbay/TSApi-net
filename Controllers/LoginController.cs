
//using MindQ.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using TSApi;
using TSApi.Authentication;
using TSTypes.Requests;

namespace TSApi.Controllers
{
    public class LoginController : ApiController
    {


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Login/Auth/{username}/{password}")]
        public HttpResponseMessage Login(string username,
                                   string password)
        {

            Logger.Info(string.Format("api/Login/Auth/{0}", username));


            try
            {
                TSTypes.User u = TSDal.UserSvc.Login(username, password);
                if (u == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = "Invalid Username or password" });
                }
                
                TSTypes.User user = new TSTypes.User { Username = u.Username, Firstname = u.Firstname, Lastname = u.Lastname, UserId = u.UserId, CustomerId = u.CustomerId };

                Ticket t = TicketManager.Instance.Create(user, AuthorizationLevel.Admin);  // authorization levels not yet implemented

                return this.Request.CreateResponse(HttpStatusCode.OK, new TSTypes.Login
                {
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Username = u.Username,
                    Email = u.Email,
                    IsError = false,
                    ErrorMessage = "",
                    Token = t.Token.ToString(),
                    LoginCookie = u.LastCookieId
                });
                


                
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new { IsError = true, ErrorMessage = ee.ToString() });
            }
	    }


        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/Login/Cookie")]
        public HttpResponseMessage Login(CookieLoginRequest request)
        {

            Logger.Info(string.Format("api/Login/Cookie/{0}", request.LoginCookieId));


            try
            {
                TSTypes.User u = TSDal.UserSvc.CookieLogin(request.LoginCookieId);

                if (u.UserId > 0)
                {
                    TSTypes.User user = new TSTypes.User { Username = u.Username, Firstname = u.Firstname, Lastname = u.Lastname, UserId = u.UserId, CustomerId = u.CustomerId };

                    Ticket t = TicketManager.Instance.Create(user, AuthorizationLevel.Admin);  // authorization levels not yet implemented

                    return this.Request.CreateResponse(HttpStatusCode.OK, new TSTypes.Login
                    {
                        Firstname = u.Firstname,
                        Lastname = u.Lastname,
                        Username = u.Username,
                        Email = u.Email,
                        IsError = false,
                        ErrorMessage = "",
                        Token = t.Token.ToString(),
                        LoginCookie = Convert.ToString(u.LastCookieId)
                    });
                }


                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = "Invalid Username or password" });
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new { IsError = true, ErrorMessage = ee.ToString() });
            }
        }

       
       
//        [HttpPost]
//        [Route("api/Login/ForgotPassword")]
//        public void Post(ForgotPasswordRequest request)
//        {

//            string email = request.Email;
//            Logger.Info(string.Format("ForgotPassword: Email [{0}].", email));

            
//            //https://46.4.87.34/BackOffice/Secure/ResetPassword.aspx?id=e0cadbd2-a807-414b-bafe-2f99cc9614ab
//            string resetPasswordUrl = "";
//            string BORootUrl = "";
//            try
//            {

                
//                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["BackOfficeRootUrl"]))
//                {
//                    string message = "Configuration parameter BackOfficeRootUrl is not set.  Please resolve the server configuration and try again.";
//                    Logger.Error(message);
//                    throw new HttpResponseException(new HttpResponseMessage()
//                    {
//                        StatusCode = HttpStatusCode.BadRequest,
//                        ReasonPhrase = message
//                    });
//                }

//                BORootUrl = ConfigurationManager.AppSettings["BackOfficeRootUrl"];
//                resetPasswordUrl = string.Format("{0}Secure/ResetPassword.aspx", BORootUrl);
//            }
//            catch (Exception ee)
//            {
//                string message = string.Format("Unexpected error while reading Configuration parameter BackOfficeRootUrl.  {0}  Please resolve the server configuration and try again.", ee.ToString());
//                Logger.Error(message);
//                throw new HttpResponseException(new HttpResponseMessage()
//                {
//                    StatusCode = HttpStatusCode.BadRequest,
//                    ReasonPhrase = message
//                });
//            }

//            TSTypes.User u;
//            try
//            {

//                // check if email relates to a User of the system
//                u = TSDal.UserSvc.GetByEmail(email);
                

//            }
//            catch (Exception ee)
//            {
//                string message = MethodBase.GetCurrentMethod().Name + ": To [" + email.Trim() + "].  Exception: " + ee.ToString();
//                Logger.Error(message);
//                throw new HttpResponseException(new HttpResponseMessage()
//                {
//                    StatusCode = HttpStatusCode.BadRequest,
//                    ReasonPhrase = message
//                });
//            }
//            if (u == null)
//            {
//                string message = string.Format("Email [{0}] count not be found.", email);
//                Logger.Error(message);
//                throw new HttpResponseException(new HttpResponseMessage()
//                {
//                    StatusCode = HttpStatusCode.BadRequest,
//                    ReasonPhrase = message
//                });
//            }


//            PasswordReset pr;
//            try
//            {
//                // get the password reset link
//                pr = PasswordReset.Add(email);
//            }
//            catch (Exception ee)
//            {
//                string message = MethodBase.GetCurrentMethod().Name + ": To [" + email.Trim() + "].  Exception: " + ee.ToString();
//                Logger.Error(message);
//                throw new HttpResponseException(new HttpResponseMessage()
//                {
//                    StatusCode = HttpStatusCode.BadRequest,
//                    ReasonPhrase = message
//                });
//            }


//            try
//            {
//                string mailSubject = "mindQ שינוי סיסמא";
//                string bodyInsert = "<div>mindQ שינוי סיסמא</div><div>Click here to reset your password: <a href='" + resetPasswordUrl + "?id=" + pr.Link + "'>" + pr.Link + "</a></div>";
//                string mailBody = @"<HTML>
//                                    <HEAD>
//                                        <META http-equiv='Content-Type' content='text/html; charset=Windows-1255'><LINK rel='stylesheet' href='https://www.derugim.co.il/CSS/general.css' type='text/css'><TITLE>mindQ שינוי סיסמא</TITLE>
//                                    </HEAD>
//                                <BODY dir='ltr' style='background-color: #EDF1FA'>
//                                    <center>
//                                    <TABLE border=0 cellspacing=0 cellpadding=0>
//                                    <TR><TD><TABLE border=0 cellspacing=0 cellpadding=0>
//                                                <TR><TD><img src='http://mindqsurvey.com/images/index_02.jpg' /><BR></TD></TR>
//                                            </TABLE>	
//                                    </TD></TR>
//                                    <TR><TD  >&nbsp; </TD></TR><TR><TD  class=formTitle2  align=right dir=rtl>" +
//                                        bodyInsert + @"</TD></TR><TR><TD align=right >&nbsp; </TD></TR>
//                                    <TR><TD align=center> 
//                                        <table border=0 cellspacing=0 cellpadding=2 align=center >
//                                            <tr align=center>
//                                                <td dir=rtl colspan=4 class=formRights> כל הזכויות שמורות לכלים שלובים בעמ </td>
//                                            </tr>
//                                           
//                                            <TR style='align: center; margin-top: 20px; '>
//                                                <TD colspan=4 ><a href='http://www.mindqsurvey.com'><FONT color=#8e9432 size=1><u>www.mindqsurvey.com</u></FONT></a>
//                                                </TD>
//                                            </TR>
//                                         </table>
//                                        </TD></TR>
//                                    </TABLE></senter>
//                                </BODY>
//                                </HTML>";


//                // read mail params from config file
//                MailConfig mConfig = new MailConfig();
//                if (mConfig.IsError)
//                {
//                    Logger.Info(MethodBase.GetCurrentMethod().Name + ": Configuration error - message [" + mConfig.ErrorMessage + "].");
//                    string message = "Mail configuration error on the MindQ server.  Please contact MindQ technical support.";
//                    throw new HttpResponseException(new HttpResponseMessage()
//                    {
//                        StatusCode = HttpStatusCode.BadRequest,
//                        ReasonPhrase = message
//                    });
//                }

//                MailService ms = new MailService(mConfig.smtpServer,
//                                                                mConfig.mailUsername,
//                                                                mConfig.mailPassword,
//                                                                mConfig.mailFromAddress,
//                                                                mConfig.mailFromName);

//                Logger.Info(mConfig.mailFromAddress);

//                // test only
//                //recipientlist = "ygubbay@gmail.com; ";

//                //List<string> toList = new List<string>();
//                //toList.Add(email.Trim());

//                ms.SendHtml(email.Trim(), mailSubject, mailBody);
//                //ms.SendHtmlWithAttachment(toList, new List<string>(), mailSubject, mailBody, true, mConfig.IsEnableSSL, mConfig.securePort, filename);
//                Logger.Info("Password reset link sent to [" + email.Trim() + "].");
//            }
//            catch (Exception ee)
//            {
//                string message = MethodBase.GetCurrentMethod().Name + ": To [" + email.Trim() + "].  Exception: " + ee.ToString();
//                Logger.Error(message);
//                throw new HttpResponseException(new HttpResponseMessage()
//                {
//                    StatusCode = HttpStatusCode.BadRequest,
//                    ReasonPhrase = ee.Message
//                });
//            }

//            return; //"Password link was sent successfully.  Check your email.";
//        }
    }


    public class ForgotPasswordRequest
    {

        public string Email { get; set; }
    }

        
}
