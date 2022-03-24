using cisApp.Function;
using cisApp.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Models
{
    public class FilePageModel
    {
        public PaginatedList<AlbumImageModel> ImageModel { get; set; }
        public string SearchBy { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
        public Guid? Designer { get; set; }
        public int? AlbumId { get; set; }
    }
}
