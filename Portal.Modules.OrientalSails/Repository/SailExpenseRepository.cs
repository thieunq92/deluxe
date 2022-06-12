using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class SailExpenseRepository:RepositoryBase<SailExpense>
    {
        public SailExpenseRepository() : base() { }
        public SailExpenseRepository(ISession session) : base(session) { }
    }
}