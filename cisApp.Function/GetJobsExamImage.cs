using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using cisApp.Core;
using cisApp.library;

namespace cisApp.Function
{
    public static class GetJobsExamImage
    {
        public class Get
        {
            public static List<JobsExamImage> GetAll()
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsExamImage.ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobsExamImage> GetByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.JobsExamImage.Where(o => o.JobId == id).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static List<JobsExamImageModel> GetImageByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = (from jimg in context.JobsExamImage.Where(o => o.JobId == id)
                                   join timg in context.JobsExamType on jimg.JobsExTypeId equals timg.JobsExTypeId into jmap
                                   from jtmap in jmap.DefaultIfEmpty()
                                   join aimg in context.AttachFile.Where(o => o.IsActive == true) on jimg.JobsExImgId equals aimg.RefId into amap
                                   from ajmap in amap.DefaultIfEmpty()
                                   select new JobsExamImageModel()
                                   {
                                       JobsExTypeId = jimg.JobsExTypeId,
                                       JobsExTypeDesc = jtmap.Description,
                                       RefId = ajmap.RefId,
                                       FileName = ajmap.FileName,
                                       Path = ajmap.Path,
                                       Size = ajmap.Size, 
                                   }).ToList();
                        if(data != null && data.Count > 0)
                            return data;
                        return new List<JobsExamImageModel>();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



        }

        public class Manage
        {
            


        }

    }
}
