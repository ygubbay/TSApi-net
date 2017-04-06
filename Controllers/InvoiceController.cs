using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TSApi.Authentication;
using TSDal;
using TSTypes;
using TSTypes.Requests;
using TSTypes.Responses;

namespace TSApi.Controllers
{
    public class InvoiceController : ApiController
    {
        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/Invoice")]
        public HttpResponseMessage CreateInvoice(InvoiceCreateRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);


                DateTime invoiceDate = new DateTime(request.InvoiceDate.Year,
                                                    request.InvoiceDate.Month,
                                                    request.InvoiceDate.Day);

                int invoiceid = InvoiceSvc.Create(t.CustomerId, request.ProjectId, request.InvoiceYear, request.InvoiceMonth, request.IsMonthly, request.InvoiceDate, request.TSEntries);

                return this.Request.CreateResponse(HttpStatusCode.OK, new CreateInvoiceResponse { InvoiceId = invoiceid, IsError = false });
            }
            catch (Exception ee)
            {
                Logger.Error("Invoice.Create: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }




        }

        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/Invoice/Save")]
        public HttpResponseMessage SaveInvoice(InvoiceSaveRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                Invoice i = InvoiceSvc.GetById(request.InvoiceId);

                if (i.CustomerId != t.CustomerId)
                {
                    Logger.Info("api/Invoice/Save: Unauthorized request");
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }

                InvoiceSvc.Save(request.InvoiceId, request.StatusId);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ee)
            {
                Logger.Error("SaveInvoice: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }




        }

        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/Get")]
        public HttpResponseMessage GetInvoice(InvoiceByIdRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                Invoice invoice = InvoiceSvc.GetById(request.InvoiceId);

                // verify authorization
                if (t.CustomerId != invoice.CustomerId)
                {
                    Logger.Error(string.Format("GetInvoice: Unauthorized request: User [{0}], requested invoiceid [{1}] which he does not own.", t.UserId, request.InvoiceId));
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, invoice);
            }
            catch (Exception ee)
            {
                Logger.Error("Invoice.Create: " + ee.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ee.Message);
            }
        }



        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/Entries/Get")]
        public HttpResponseMessage GetInvoiceEntries(InvoiceByIdRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);



                // verify authorization
                Invoice invoice = InvoiceSvc.GetById(request.InvoiceId);
                if (t.CustomerId != invoice.CustomerId)
                {
                    Logger.Error(string.Format("GetInvoice: Unauthorized request: User [{0}], requested invoiceid [{1}] which he does not own.", t.UserId, request.InvoiceId));
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }

                List<InvoiceEntry> items = InvoiceEntrySvc.GetByInvoiceId(request.InvoiceId);

                return this.Request.CreateResponse(HttpStatusCode.OK, items);
            }
            catch (Exception ee)
            {
                Logger.Error("Invoice.Create: " + ee.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ee.Message);
            }
        }


        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Invoice/Status/All")]
        public HttpResponseMessage GetInvoiceStatuses()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, InvoiceSvc.GetAllStatuses());

        }


        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/Pdf")]
        public HttpResponseMessage CreatePdfInvoice(ExcelInvoiceRequest request)
        {

            try
            {
                Logger.Info(string.Format("{0}: Request [{1}]", this.Request.GetRequestContext().Url, JsonConvert.SerializeObject(request)));

                Ticket t = TicketManager.Instance.GetTicket(request.Token);
                if (t == null)
                {
                    Logger.Info(string.Format("api/Invoice/Pdf: GetTicket failed for token [{0}]", request.Token));
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }
                Invoice i = InvoiceSvc.GetById(request.InvoiceId);

                if (t.CustomerId != t.CustomerId)
                {
                    Logger.Info(string.Format("Invoice/Pdf: Unauthorized request by User {0}", t.Username));
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }
                Project project = ProjectSvc.GetById(t.CustomerId, i.ProjectId);
                Customer c = TSDal.CustomerSvc.GetById(t.CustomerId);

                string downloadsFolder = ConfigurationManager.AppSettings["downloadsFolder"];
                string downloadsRootUrl = ConfigurationManager.AppSettings["downloadsRootUrl"];
                string ProjectAbbrev = project.CustCompanyName.Substring(0, 2);
                if (!string.IsNullOrEmpty(project.InvoiceFilePrefix))
                {
                    ProjectAbbrev = project.InvoiceFilePrefix;
                }

                // get output filename
                string downloadFile = string.Format("{0} invoice {1}-{2}.pdf", ProjectAbbrev, i.PeriodYear, i.PeriodMonth);
                downloadFile = GetUniqueDownloadFile(downloadsFolder, downloadFile, "pdf");
                string downloadPathFile = string.Format("{0}/{1}", downloadsFolder, downloadFile);

                List<InvoiceEntry> invoiceItems = InvoiceEntrySvc.GetByInvoiceId(i.Id);
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(c.InvoiceCultureName);

                InvoicePdfCreateRequest req = new InvoicePdfCreateRequest
                {

                    InvoiceCulture = cultureInfo,
                    CurrencySymbol = c.CurrencySymbol,
                    InvoiceDate = i.InvoiceDate,
                    Project = project,
                    InvoiceBase = i,
                    Customer = c,
                    Items = invoiceItems,
                    Filename = downloadPathFile
                };


                //report.Create();
                PdfRunner.PdfRunnerResult result = PdfRunner.Runner.Execute(req);

                // download link
                return this.Request.CreateResponse(HttpStatusCode.OK, new ReportResponse
                {
                    IsError = false,
                    Url = string.Format("{0}/{1}", downloadsRootUrl, downloadFile),
                    Filename = downloadFile,
                    ErrorMessage = ""
                });
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }

        }




        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/ExportExcel")]
        public HttpResponseMessage CreateExcelInvoice(ExcelInvoiceRequest request)
        {

            try
            {
                Logger.Info("api/Invoice/ExportExcel");

                Ticket t = TicketManager.Instance.GetTicket(request.Token);
                if (t == null)
                {
                    Logger.Info(string.Format("api/Invoice/ExportExcel: GetTicket failed for token [{0}]", request.Token));
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }


                Invoice i = InvoiceSvc.GetById(request.InvoiceId);

                if (t.CustomerId != t.CustomerId)
                {
                    Logger.Info(string.Format("Invoice/ExportExcel: Unauthorized request by User {0}", t.Username));
                    return this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }
                Project project = ProjectSvc.GetById(t.CustomerId, i.ProjectId);
                string downloadsFolder = ConfigurationManager.AppSettings["downloadsFolder"];
                string downloadsRootUrl = ConfigurationManager.AppSettings["downloadsRootUrl"];
                string ProjectAbbrev = project.CustCompanyName.Substring(0, 2);
                if (!string.IsNullOrEmpty(project.InvoiceFilePrefix))
                {
                    ProjectAbbrev = project.InvoiceFilePrefix;
                }

                // get output filename
                string downloadFile = string.Format("{0} ts {1}-{2}.xlsx", ProjectAbbrev, i.PeriodYear, i.PeriodMonth);
                downloadFile = GetUniqueDownloadFile(downloadsFolder, downloadFile, "xlsx");
                string downloadPathFile = string.Format("{0}/{1}", downloadsFolder, downloadFile);




                // copy from template to downloads folder
                string templateVirFile = "/Templates/timesheet1.xlsx";
                string templateFile = HttpContext.Current.Server.MapPath(templateVirFile);
                Logger.Info(string.Format("Copy file [{0}] to [{1}] ", templateFile, downloadPathFile));
                System.IO.File.Copy(templateFile, downloadPathFile);


                TSExcelReport.InvoiceTemplate tpl = new TSExcelReport.InvoiceTemplate();

                tpl.ClientCompanyName = new TSExcelReport.Cell(4, 2);
                tpl.ConsultantName = new TSExcelReport.Cell(5, 2);
                tpl.ClientPersonName = new TSExcelReport.Cell(6, 2);
                tpl.InvoiceMonth = new TSExcelReport.Cell(7, 2);
                tpl.InvoiceItemsStart = new TSExcelReport.Cell(10, 1);
                tpl.TotalDuration = new TSExcelReport.Cell(13, 3);

                tpl.HourlyRate = new TSExcelReport.Cell(15, 2);
                tpl.TotalTimeAmountCalculated = new TSExcelReport.Cell(15, 3);

                tpl.TotalExTax = new TSExcelReport.Cell(21, 3);
                tpl.TaxRate = new TSExcelReport.Cell(22, 3);
                tpl.TotalIncTax = new TSExcelReport.Cell(24, 3);
                tpl.CurrSymbol = TSExcelReport.CurrencySymbol.Shekel;

                //TSExcelReport.Invoice report = new TSExcelReport.Invoice(t.CustomerId,
                //                                                            tpl,
                //                                                            i,
                //                                                            downloadPathFile);
                //report.Create();

                // download link
                return this.Request.CreateResponse(HttpStatusCode.OK, new ReportResponse
                {
                    IsError = false,
                    Url = string.Format("{0}/{1}", downloadsRootUrl, downloadFile),
                    Filename = downloadFile,
                    ErrorMessage = ""
                });
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }

        }


        private string GetUniqueDownloadFile(string path, string origfile, string ext)
        {
            string curfile = origfile;
            string origFilePath = string.Format("{0}/{1}", path, curfile);
            string curfilePath = origFilePath;
            var i = 1;
            while (System.IO.File.Exists(curfilePath))
            {

                curfile = string.Format("{0}_{1}.{2}", System.IO.Path.GetFileNameWithoutExtension(origFilePath), i++, ext);
                curfilePath = string.Format("{0}/{1}", path, curfile);
            }
            return curfile;
        }


        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/All")]
        public HttpResponseMessage AllInvoices(AllInvoicesRequest request)
        {
            try
            {
                Logger.Info(string.Format("api/Invoice/All: ProjectId={0}, PageIndex={1}, PageCount={2}", request.ProjectId, request.PageIndex, request.PageCount));

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                return this.Request.CreateResponse(HttpStatusCode.OK, TSDal.InvoiceSvc.AllInvoices(t.CustomerId,
                                                    request.ProjectId,
                                                    request.FromDate,
                                                    request.ToDate,
                                                    request.PageIndex,
                                                    request.PageCount));
            }
            catch (Exception ee)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
        }


        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/Last")]
        public HttpResponseMessage LastInvoices(LastInvoicesRequest request)
        {
            try
            {
                Logger.Info(string.Format("api/Invoice/Last: ProjectId {0}, Status {1}, PageIndex={2}, PageCount={3}", request.ProjectId, request.StatusId, request.PageIndex, request.PageCount));

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                return this.Request.CreateResponse(HttpStatusCode.OK, TSDal.InvoiceSvc.LastInvoices(t.CustomerId,
                                                    request.ProjectId,
                                                    request.StatusId,
                                                    request.PageIndex,
                                                    request.PageCount));
            }
            catch (Exception ee)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/LastByProject")]
        public HttpResponseMessage LastByProject(LastInvoicesByProjectRequest request)
        {
            try
            {
                Logger.Info(string.Format("api/Invoice/LastByProject: ProjectId {0}, PageIndex={1}, PageCount={2}", request.ProjectId, request.PageIndex, request.PageCount));

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                return this.Request.CreateResponse(HttpStatusCode.OK, TSDal.InvoiceSvc.LastInvoicesByProject(t.CustomerId,
                                                    request.ProjectId,
                                                    request.PageIndex,
                                                    request.PageCount));
            }
            catch (Exception ee)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Invoice/Logo")]
        public HttpResponseMessage CreateLogo(CreateLogoRequest request)
        {
            try
            {
                Logger.Info(string.Format("{0}", this.Request.GetRequestContext().Url));

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                TSDal.InvoiceSvc.SaveLogo(t.CustomerId, request.Filename);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { status = "OK" });
            }
            catch (Exception ee)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
        }
    }
}
