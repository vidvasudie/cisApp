﻿using cisApp.library;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace cisApp.Function
{
    public class SearchModel
    {
        public Guid? Id { get; set; }
        public Guid? gId { get; set; }
        public string text { get; set; }
        public int? currentPage { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
        public int status { get; set; } = 0;
        public int mode { get; set; } = 0; 
        public int? type { get; set; }
        public string goBack { get; set; }
        public bool? active { get; set; }
        public string statusOpt { get; set; }
        public string statusStr { get; set; }
        public string searchText { get; set; }

        public int? jobType { get; set; }
        public int? PaymentStatus { get; set; }
        public Guid? JobId { get; set; }
        public int? JobStatus { get; set; }

        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }

        public Guid? SenderId { get; set; }
        public Guid? RecieverId { get; set; }

        public List<Guid> Imgs { get; set; }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public string Tags { get; set; }
        public string Categories { get; set; }
        public string Orderby { get; set; }
        public string SearchBy { get; set; }
        public int? AlbumId { get; set; }
        public Guid? Designer { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartDateStr
        {
            get
            {
                CultureInfo cultureInfo = new CultureInfo("th-TH");
                return (StartDate != null) ? StartDate.Value.ToString("dd/MM/yyyy", cultureInfo) : "";
            }
            set
            {
                StartDate = value.ToDateTimeFormat();
            }
        }
        public string EndDateStr
        {
            get
            {
                CultureInfo cultureInfo = new CultureInfo("th-TH");
                return (EndDate != null) ? EndDate.Value.ToString("dd/MM/yyyy", cultureInfo) : "";
            }
            set
            {
                EndDate = value.ToDateTimeFormat();
            }
        }

        public bool? IsPaid { get; set; }
        public string RefCode { get; set; }
        public string OrderBy { get; set; }
    }
}
