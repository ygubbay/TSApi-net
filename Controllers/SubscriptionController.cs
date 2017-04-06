using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TSApi.Authentication;
using TSDal;
using TSTypes.Requests.Subscriptions;

namespace TSApi.Controllers
{
    public class SubscriptionController: ApiController
    {

        public TSTypes.User RegisterTrial()
        {
            return new TSTypes.User();
        }


        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Subscriptions/CustomerSave")]
        public HttpResponseMessage CustomerSave(CustomerSaveRequest request)
        {
            try
            {

                Logger.Info(string.Format("{0}: request [{1}]", this.Request.GetRequestContext().Url, JsonConvert.SerializeObject(request)));

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);
                request.Customer.Id = t.CustomerId;

                CustomerSvc.CustomerSave(request.Customer);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { status = "OK" });
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ee.ToString() });
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Subscriptions/Customer/{Token}")]
        public HttpResponseMessage CustomerGet(string Token)
        {
            try
            {

                Logger.Info(string.Format("{0}: request [{1}]", this.Request.GetRequestContext().Url, Token));

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(Token);

                return this.Request.CreateResponse(HttpStatusCode.OK, CustomerSvc.GetById(t.CustomerId));
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ee.ToString() });
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Subscriptions/Languages")]
        public HttpResponseMessage LanguagesGet()
        {
            try
            {

                Logger.Info(string.Format("{0}", this.Request.GetRequestContext().Url));

                return this.Request.CreateResponse(HttpStatusCode.OK, CustomerSvc.GetLanguages());
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ee.ToString() });
            }
        }

    }
}