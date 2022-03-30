﻿using cisApp.Core;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetNotification
    {
        public class Get
        {
            public static List<Notification> GetbyUserID(Guid userID)
            {
                using (var context = new CAppContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {

                        return context.Notification.Where(o => o.UserId == userID && o.IsActive == true).ToList(); 

                    }
                } 
            }

        }
        public class Manage
        {
            public static void add(Guid UserID,string Url, string title , string Msg)
            {
                using (var context = new CAppContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        Notification obj = new Notification();

                        obj.UserId = UserID;
                        obj.Title = title;
                        obj.Msg = Msg;
                        obj.IsActive = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.IsRead = false;
                        obj.ReadDate = null;
                        context.Notification.Add(obj);
                        context.SaveChanges(); 
                    }
                }

            }

            public static void Read(int ID,Guid userId)
            {

                using (var context = new CAppContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        Notification obj = context.Notification.Find(ID);
                        if (userId == obj.UserId)
                        {
                            obj.IsRead = true;
                            obj.ReadDate = DateTime.Now;
                            context.Notification.Add(obj);
                            context.SaveChanges();
                        }
                    }
                }
            }

        }

    }
}