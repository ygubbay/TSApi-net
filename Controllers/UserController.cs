using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TSApi.Authentication;
using TSDal;
using TSTypes.Requests;

namespace TSApi.Controllers
{
    public class UserController: ApiController
    {

        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Users/Registration")]
        public HttpResponseMessage Registration(RegistrationRequest request)
        {
            try
            {

                
                
                TSTypes.User user = UserSvc.Registration(request);


                return this.Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ee)
            {
                Logger.Error("Registration: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }

        }

        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Users/{token}")]
        public HttpResponseMessage GetUserProfile(string token)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);


                TSTypes.User u = UserSvc.GetByUserId(t.UserId);


                return this.Request.CreateResponse(HttpStatusCode.OK, u);
            }
            catch (Exception ee)
            {
                Logger.Error("GetUserProfile: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }

        }



        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Users")]
        public HttpResponseMessage SaveUserProfile(SaveUserProfileRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);


                UserSvc.Save(t.UserId, 
                                request.Firstname, 
                                request.Lastname,
                                request.Email);


                return this.Request.CreateResponse(HttpStatusCode.OK, new { IsError = false });
            }
            catch (Exception ee)
            {
                Logger.Error("GetUserProfile: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }

        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Users/Currencies")]
        public HttpResponseMessage GetCurrencies()
        {
            try
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, CurrencySvc.GetCurrencies());
            }
            catch (Exception ee)
            {
                Logger.Error("api/Users/Currencies" + ee.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Users/Alerts/{token}")]
        public HttpResponseMessage GetAlerts(string token)
        {
            try
            {
                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                return this.Request.CreateResponse(HttpStatusCode.OK, CustomerSvc.GetAlerts(t.CustomerId));
            }
            catch (Exception ee)
            {
                Logger.Error(string.Format("{0}: {1}", this.Request.GetRequestContext().Url.ToString(), ee.ToString()));
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
        }


    }
}