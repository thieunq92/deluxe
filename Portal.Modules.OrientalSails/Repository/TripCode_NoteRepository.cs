using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Repository
{
    public class TripCode_NoteRepository : RepositoryBase<TripCode_Note>
    {
        public TripCode_NoteRepository() : base() { }

        public List<TripCode_Note> TripCode_NoteGetAllByTripCode(string tripCode)
        {
            var query = _session.QueryOver<TripCode_Note>().Where(tcn => tcn.TripCode == tripCode);
            return query.Future().ToList();
        }

        public int TripCode_NoteGetCountOfNewNotes(string tripCode, DateTime? lastCheck)
        {
            var query = _session.QueryOver<TripCode_Note>().Where(tcn => tcn.TripCode ==  tripCode && tcn.CreatedDate > lastCheck);
            return query.RowCount();
        }
    }
}