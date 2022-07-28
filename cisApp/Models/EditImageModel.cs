using cisApp.Core;
using cisApp.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.Models
{
    public class EditImageModel
    {

        public List<AlbumImageModel> Images { get; set; }


        public AttachFile AttachFileImage { get; set; }

        public Guid AttachFileId { get; set; }

        public bool FileRemove { get; set; }


        public string FileBase64 { get; set; }


        public string FileName { get; set; }


        public string FileSize { get; set; }

        public string UrlPath
        {
            get
            {
                return "~/Uploads" + "/" + this.AttachFileId + "/" + this.FileName;
            }

        }
    }
    }
