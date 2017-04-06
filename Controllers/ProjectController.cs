using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSApi.Authentication;
using TSDal;
using TSDal.TSDal;
using TSTypes;
using TSTypes.Requests;

namespace TSApi.Controllers
{
    public class ProjectController : ApiController
    {

        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Projects/{token}")]
        public HttpResponseMessage GetProjectsByUser(string token)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                List<Project> l = ProjectSvc.GetByCustomer(t.CustomerId);

                return this.Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ee)
            {
                Logger.Error("GetProjectsByUser: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        }

        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/ProjectsPlus/{token}")]
        public HttpResponseMessage GetProjectsPlus(string token)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                List<ProjectPlus> l = ProjectPlusSvc.Get(t.CustomerId);

                return this.Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ee)
            {
                Logger.Error("GetProjectsByUser: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/ProjectsPlus/{token}/{year}")]
        public HttpResponseMessage GetProjectsPlus(string token, int year)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                List<ProjectPlus> l = ProjectPlusSvc.Get(t.CustomerId, year);

                return this.Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ee)
            {
                Logger.Error("GetProjectsByUser: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Projects/Active/{token}")]
        public HttpResponseMessage GetActiveProjectsByUser(string token)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                List<Project> l = ProjectSvc.GetActiveByCustomer(t.CustomerId);

                return this.Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ee)
            {
                Logger.Error("GetActiveProjectsByUser: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Projects/{token}/{projectid}")]
        public HttpResponseMessage GetProjectById(string token, int projectid)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                Project p = ProjectSvc.GetById(t.CustomerId, projectid);

                return this.Request.CreateResponse(HttpStatusCode.OK, p);
            }
            catch (Exception ee)
            {
                Logger.Error("GetProjectById: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }




        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Projects/Rate/{token}/{projectid}")]
        public HttpResponseMessage GetRateByProjectId(string token, int projectid)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(token);

                List<HourlyRate> rates = HourlyRateSvc.GetByProjectId(projectid);

                return this.Request.CreateResponse(HttpStatusCode.OK, rates[0]);
            }
            catch (Exception ee)
            {
                Logger.Error("GetProjectById: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }




        }



        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Projects/CreateWizard")]
        public HttpResponseMessage CreateProject(CreateProjectWizardRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);
                if (t == null)
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized Access");
                }

                string prefixName = request.Name.Length > 1 ? request.Name.Substring(0, 2) : "";
                //
                // Currently we default the Culturekey to hebrew 
                // -> this should be read from Customer setting
                //
                int projectid = ProjectSvc.Create(t.CustomerId,
                                            request.Name,
                                            request.PaymentTermsDays,
                                            request.SalesTax,
                                            request.CompanyName,
                                            request.ContactName,
                                            request.ContactEmail,
                                            request.ContactPhone,
                                            "he",
                                            prefixName);
                

                HourlyRateSvc.Create(projectid, t.UserId, request.HourlyRate);
                return this.Request.CreateResponse(HttpStatusCode.OK, 0);

                    
            }
            catch (Exception ee)
            {
                Logger.Error("CreateProject: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiError
                {
                    Message = ee.Message,
                    ApiFunction = "CreateProject",
                    HttpErrorCode = (int)HttpStatusCode.Unauthorized
                });

            }
        }



        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Projects/Update")]
        public HttpResponseMessage UpdateProject(UpdateProjectRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);


                ProjectSvc.Update(t.CustomerId,
                                    request.ProjectId,
                                    request.Name,
                                    request.PaymentTermsDays,
                                    request.SalesTax,
                                    request.InvoiceFilePrefix,
                                    request.CompanyName,
                                    request.ContactName,
                                    request.ContactEmail,
                                    request.ContactPhone);

                HourlyRateSvc.Update(t.CustomerId,
                                     request.ProjectId,
                                     request.HourlyRate);

                return this.Request.CreateResponse(HttpStatusCode.OK,
                                                    new { IsError = false });
            }
            catch (Exception ee)
            {
                Logger.Error("UpdateProject: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiError { Message = ee.Message, 
                                                                                               ApiFunction = "CreateProject",
                                                                                               HttpErrorCode = (int)HttpStatusCode.Unauthorized
                });
          
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Projects")]
        public HttpResponseMessage CreateProject(CreateProjectRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);


                //
                // Currently we default the Culturekey to hebrew 
                // -> this should be read from Customer setting
                //
                return this.Request.CreateResponse(HttpStatusCode.OK, 
                    
                    

                    ProjectSvc.Create(t.CustomerId,
                                            request.Name,
                                            request.PaymentTermsDays,
                                            request.SalesTax,
                                            request.CustCompanyName,
                                            request.CustContactName,
                                            request.CustContactEmail,
                                            request.CustContactPhone,
                                            "he",
                                            ""));
            }
            catch (Exception ee)
            {
                Logger.Error("CreateProject: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiError { Message = ee.Message, 
                                                                                               ApiFunction = "CreateProject",
                                                                                               HttpErrorCode = (int)HttpStatusCode.Unauthorized
                });
          
            }
        }


        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Projects/Links")]
        public HttpResponseMessage CreateProjectLink(CreateProjectLinkRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                Project p = ProjectSvc.GetById(t.CustomerId, request.ProjectId);
                if (p == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "You are not authorized for this project.");
                }

                ProjectLink pl = ProjectLinkSvc.New(request.ProjectId);

                return this.Request.CreateResponse(HttpStatusCode.OK, pl);

            }
            catch (Exception ee)
            {

                Logger.Error(string.Format("{0}: {1}", this.Request.GetRequestContext().Url.ToString(), ee.ToString()));

                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiError
                {
                    Message = ee.Message,
                    ApiFunction = this.Request.GetRequestContext().Url.ToString(),
                    HttpErrorCode = (int)HttpStatusCode.Unauthorized
                });

            }
        }
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Projects/Links/{link}")]
        public HttpResponseMessage GetProjectLinkView(string link)
        {
            try
            {


                Project p = ProjectSvc.GetByProjectLinkId(link);
                if (p == null)
                {
                    string msg = string.Format("Invalid link {0}.", link);
                    Logger.Error(msg);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, msg);
                }

                List<TimeEntry> l = TSSvc.GetUninvoicedTEByProjectId(p.ProjectId);

                return this.Request.CreateResponse(HttpStatusCode.OK, l);

            }
            catch (Exception ee)
            {

                Logger.Error(string.Format("{0}: {1}", this.Request.GetRequestContext().Url.ToString(), ee.ToString()));

                // this can occur if User sets this to inactive
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, new ApiError
                {
                    Message = "Check with the Link provider that the Link is still active.",
                    ApiFunction = this.Request.GetRequestContext().Url.ToString(),
                    HttpErrorCode = (int)HttpStatusCode.Forbidden
                });

            }
        }

        // Project links by customer
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Projects/Links/ByCustomer")]
        public HttpResponseMessage GetProjectLinksByCustomer(GetProjectLinksByCustomerRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                List<ProjectLink> l = ProjectLinkSvc.GetByCustomer(t.CustomerId);

                return this.Request.CreateResponse(HttpStatusCode.OK, l);

            }
            catch (Exception ee)
            {

                Logger.Error(string.Format("{0}: {1}", this.Request.GetRequestContext().Url.ToString(), ee.ToString()));

                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiError
                {
                    Message = ee.Message,
                    ApiFunction = this.Request.GetRequestContext().Url.ToString(),
                    HttpErrorCode = (int)HttpStatusCode.Unauthorized
                });

            }
        }



        //  Update project link
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Projects/Links/Update")]
        public HttpResponseMessage UpdateProjectLinks(UpdateProjectLinksRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                List<ProjectLink> l = ProjectLinkSvc.GetByCustomer(t.CustomerId);

                ProjectLink p = l.Find(pl => pl.ProjectId == request.ProjectId);
                if (p == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Project {0} does not exist or is not owned by Customer.");
                }

                ProjectLinkSvc.Update(p.Id, request.IsActive);

                return this.Request.CreateResponse(HttpStatusCode.OK, "OK");

            }
            catch (Exception ee)
            {

                Logger.Error(string.Format("{0}: {1}", this.Request.GetRequestContext().Url.ToString(), ee.ToString()));

                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiError
                {
                    Message = ee.Message,
                    ApiFunction = this.Request.GetRequestContext().Url.ToString(),
                    HttpErrorCode = (int)HttpStatusCode.Unauthorized
                });

            }
        }


    }
}
