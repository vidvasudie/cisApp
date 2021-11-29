using cisApp.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace cisApp.Function
{
    public static class GetAlbum
    {
        static string _UploadDir = "Uploads";
        public class Get
        {
            public static List<Album> GetByJobId(Guid id)
            {
                try
                {
                    using (var context = new CAppContext())
                    {
                        var data = context.Album.Where(o => o.JobId == id && o.IsActive == true && o.IsDeleted == false).ToList();

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }                
            }

            public static List<AlbumModel> GetJobCandidateAlbum(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<AlbumModel>("GetJobCandidateAlbum", parameter);
                }
                catch (Exception ex)
                {
                    return new List<AlbumModel>();
                }
            }

            public static List<AlbumModel> GetJobSubmitAlbum(SearchModel model)
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[] {
                       new SqlParameter("@jobId", model.gId != null && model.gId != Guid.Empty ? model?.gId : (object)DBNull.Value),
                       new SqlParameter("@albumType", model.type != null ? model?.type : (object)DBNull.Value)
                    };

                    return StoreProcedure.GetAllStored<AlbumModel>("GetJobSubmitAlbum", parameter);
                }
                catch (Exception ex)
                {
                    return new List<AlbumModel>();
                }
            }

        }

        public class Manage
        {
             
        }
    }
}
