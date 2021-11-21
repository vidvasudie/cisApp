using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class RoleManageModel
    {
		public Guid MenuId { get; set; }
		public string MenuName { get; set; }
		public int Level { get; set; }
		public int Type { get; set; }
		public bool IsLock { get; set; }
		public bool IsUser { get; set; }
	}
}
