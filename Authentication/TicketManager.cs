using log4net;
using TSTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSApi.Authentication
{
    public sealed class TicketManager
    {

        private static volatile TicketManager instance;
        private static object syncRoot = new Object();

        private TicketManager() {

            _tickets = new List<Ticket>();
        }

        public static TicketManager Instance
        {
            get 
            {
                if (instance == null) 
                {
                    lock (syncRoot) 
                    {
                        if (instance == null)
                            instance = new TicketManager();
                    }
                }

                return instance;
            }
        }


        public List<Ticket> _tickets;
        static readonly object _ticketLock = new object();


        public Ticket Create(User user, AuthorizationLevel authorization)
        {
            Ticket newTicket = new Ticket();

            lock (_ticketLock)
            {
                // remove expired tickets
                List<Ticket> deleteTickets = new List<Ticket>();
                foreach (Ticket t in _tickets)
                {
                    if (t.ExpiryDate < DateTime.Now)
                    {
                        deleteTickets.Add(t);
                    }

                }
                foreach (Ticket t in deleteTickets)
                {
                    _tickets.Remove(t);
                }


                

                newTicket = new Ticket
                {
                    Token = Guid.NewGuid(),
                    UserId = user.UserId,
                    Username = user.Username,
                    CustomerId = user.CustomerId,
                    LoginDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.Add(new TimeSpan(0, 60, 0)),
                    Authorization = authorization
                };

                
                _tickets.Add(newTicket);
                
            }

            return newTicket;

        }


        public int GetCustomerId(string ticketData)
        {

            try
            {
                Guid guid = new Guid(ticketData);
                return GetCustomerId(guid);
            }
            catch (Exception ee)
            {
                Logger.Error("TicketManager.GetCustomerId", ee);
                throw new Exception("TicketManager.GetCustomerId: Problem with customer authorization.");
            }
        }


        public Ticket GetTicket(string ticketKey)
        {
            try
            {
                Guid guid = new Guid(ticketKey);
                return _tickets.Find(tt => tt.Token == guid);

            }
            catch (Exception ee)
            {
                Logger.Error("TicketManager.GetTicket", ee);
                throw new Exception("TicketManager.GetTicket: Customer not authorized.");
            }
        }

        public int GetCustomerId(Guid ticketData)
        {
            try
            {
                return _tickets.Find(tt => tt.Token == ticketData).CustomerId;

            }
            catch (Exception ee)
            {
                Logger.Error("TicketManager.GetCustomerId", ee);
                throw new Exception("TicketManager.GetCustomerId: Customer not authorized.");
            }
        }


        public bool IsValid(Guid ticketData)
        {

            try
            {
                Ticket current = _tickets.Find( tt => tt.Token == ticketData);


                return (current.ExpiryDate < DateTime.Now) ? false: true;
            }
            catch (Exception ee)
            {
                Logger.Error("TicketManager.IsValid", ee);
                return false;
            }
        }

        private bool IsExists(string username, AuthorizationLevel authorization)
        {
            try
            {
                Ticket current = _tickets.First(uu => uu.Username == username && uu.Authorization == authorization);
                return (current == null) ? false : true;
            }
            catch (Exception ee)
            {
                Logger.Error("TicketManager.IsExists", ee);
                return false;
            }
        }

        private Ticket GetTicket(string username, AuthorizationLevel authorization)
        {
            try
            {
                return _tickets.First(uu => uu.Username == username && uu.Authorization == authorization);
            }
            catch (Exception ee)
            {
                Logger.Error("TicketManager.GetTicket", ee);
                throw ee; 
            }
        }
        
    }
}