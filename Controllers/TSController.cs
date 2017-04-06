using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSApi.Authentication;
using TSDal;
using TSTypes;
using TSTypes.Requests;
using TSTypes.Responses;


namespace TSApi.Controllers
{
    public class TSController : ApiController
    {
        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS")]
        public HttpResponseMessage SaveNewTS(SaveTSRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                DateTime entrydate = new DateTime(request.EntryDateYYMMDD.Year, 
                                                    request.EntryDateYYMMDD.Month, 
                                                    request.EntryDateYYMMDD.Day);
                

                //if (!DateTime.TryParseExact(request.EntryDateYYMMDD, "yyyyMMdd",  CultureInfo.InvariantCulture, DateTimeStyles.None, out entrydate))
                //{
                //    throw new Exception("Invalid date in Timesheet entry.");
                //}
                
                int startHours = Convert.ToInt32(request.StartTimeHHMM.Substring(0, 2));
                int startMinutes = Convert.ToInt32(request.StartTimeHHMM.Substring(2, 2));
                DateTime startDate = entrydate.AddHours(startHours);
                startDate = startDate.AddMinutes(startMinutes);

                int endHours = Convert.ToInt32(request.EndTimeHHMM.Substring(0, 2));
                int endMinutes = Convert.ToInt32(request.EndTimeHHMM.Substring(2, 2));
                DateTime endDate = entrydate.AddHours(endHours);
                endDate = endDate.AddMinutes(endMinutes);

                TSSvc.SaveNew(request.ProjectId,
                              entrydate,
                              startDate,
                              endDate,
                              request.Description,
                              true,
                              request.BreakMinutes,
                              t.UserId,
                              false);


                return this.Request.CreateResponse(HttpStatusCode.OK, new SaveTSResponse { IsError = false, ErrorMessage = null });
            }
            catch (Exception ee)
            {
                Logger.Error("GetProjectsByUser: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }




        }


        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS/Delete")]
        public HttpResponseMessage DeleteTS(DeleteTSRequest request)
        {
            try
            {
                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                TSSvc.Delete(request.TaskId);

                return this.Request.CreateResponse(HttpStatusCode.OK, new SaveTSResponse { IsError = false, ErrorMessage = null });

            }
            catch (Exception ee)
            {
                Logger.Error("TSController.DeleteTS: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        }

        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS/Update")]
        public HttpResponseMessage SaveTS(UpdateTSRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                DateTime entrydate = new DateTime(request.EntryDateYYMMDD.Year,
                                                    request.EntryDateYYMMDD.Month,
                                                    request.EntryDateYYMMDD.Day);


                int startHours = Convert.ToInt32(request.StartTimeHHMM.Substring(0, 2));
                int startMinutes = Convert.ToInt32(request.StartTimeHHMM.Substring(2, 2));
                DateTime startDate = entrydate.AddHours(startHours);
                startDate = startDate.AddMinutes(startMinutes);

                int endHours = Convert.ToInt32(request.EndTimeHHMM.Substring(0, 2));
                int endMinutes = Convert.ToInt32(request.EndTimeHHMM.Substring(2, 2));
                DateTime endDate = entrydate.AddHours(endHours);
                endDate = endDate.AddMinutes(endMinutes);

                TSSvc.Save(request.Id, 
                              request.ProjectId,
                              entrydate,
                              startDate,
                              endDate,
                              request.Description,
                              true,
                              request.BreakMinutes,
                              t.UserId,
                              false);


                return this.Request.CreateResponse(HttpStatusCode.OK, new SaveTSResponse { IsError = false, ErrorMessage = null });
            }
            catch (Exception ee)
            {
                Logger.Error("TSController.UpdateTS: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }

        }


        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS/DailyStats")]
        public HttpResponseMessage TSDailyStats(TSDailyStatsRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                DateTime entrydate = new DateTime(request.EntryDate.Year,
                                             request.EntryDate.Month,
                                             request.EntryDate.Day);

                TSDailyStatsResponse r = TSSvc.DailyStats(t.UserId, entrydate);


                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ee)
            {
                Logger.Error("DailyStats: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        }


        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS/ProjectUsage")]
        public HttpResponseMessage ProjectUsage(TSProjectUsageRequest request)
        {
            try
            {

                // Get userid from token
                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                DateTime entrydate = new DateTime(request.EntryDate.Year,
                                                    request.EntryDate.Month,
                                                    request.EntryDate.Day);
                TSProjectUsageResponse r = TSSvc.ProjectUsage(t.UserId, entrydate);


                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ee)
            {
                Logger.Error("ProjectUsage: " + ee.ToString());
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, new TSTypes.Login { IsError = true, ErrorMessage = ee.Message });
            }
        } 

        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS/DailyEntries")]
        public HttpResponseMessage DailyEntries(TSDailyEntriesRequest request)
        {
            
            Ticket t = TicketManager.Instance.GetTicket(request.Token);
        
            GridResponse resp = new GridResponse();

            resp.page = Convert.ToString(request.PageIndex);
            RowElement[] rows;


            // Grid definition
            int pageSize = 10;

            DateTime entrydate = new DateTime(request.EntryDate.Year,
                                                   request.EntryDate.Month,
                                                   request.EntryDate.Day);

            List<TimeEntry> tsentries = TSSvc.TSDailyEntries(t.UserId, entrydate);

            // calculate number of pages
            resp.total = Convert.ToString(tsentries.Count / pageSize + ((tsentries.Count % pageSize) > 0 ? 1 : 0));

            // Get row data
            rows = new RowElement[tsentries.Count];
            int i = 0;
            foreach (TimeEntry te in tsentries)
            {
                
                rows[i++] = new RowElement { id = te.Id.ToString(), cell = TimeEntryToRow(te) };
            }

            resp.records = Convert.ToString(tsentries.Count);
            resp.rows = rows;


            return this.Request.CreateResponse( HttpStatusCode.OK, resp);

        }



        [AcceptVerbs("POST")]
        [HttpGet]
        [Route("api/TS/BillEntries")]
        public HttpResponseMessage BillEntries(TSBillEntriesRequest request)
        {
            try
            {

                Ticket t = TicketManager.Instance.GetTicket(request.Token);

                List<TimeEntry> tsentries = TSSvc.BillEntries(request.ProjectId,
                                                                request.InvoiceYear,
                                                                request.InvoiceMonth,
                                                                request.IsMonthly);

                return this.Request.CreateResponse(HttpStatusCode.OK, tsentries);
            }
            catch (Exception ee)
            {
                Logger.Error(ee.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }

        }


        private string[] TimeEntryToRow(TimeEntry te)
        {
            return new string[] {Convert.ToString(te.Id),
                                                te.EntryDate.ToShortDateString(),
                                                te.ProjectId.ToString(),
                                                te.ProjectName,
                                                te.Description,
                                                ((DateTime)te.StartTime).ToString("HH:mm"),
                                                te.EndTime == null ? "" : ((DateTime)te.EndTime).ToString("HH:mm"),
                                                te.BreakMinutes.ToString(),
                                                te.DurationMinutes.ToString() };
        }



    }


}

