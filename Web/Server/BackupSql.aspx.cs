using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Web.Server
{
    public partial class BackupSql : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Microsoft.SqlServer.Management.Smo.Server myServer = new Microsoft.SqlServer.Management.Smo.Server(@"localhost\sqlexpress");
            myServer.ConnectionContext.LoginSecure = false;
            myServer.ConnectionContext.Login = "deluxe";
            myServer.ConnectionContext.Password = "atm123$$$";
            myServer.ConnectionContext.Connect();

            Backup bkpDBFull = new Backup();
            bkpDBFull.Action = BackupActionType.Database;
            bkpDBFull.Database = "deluxe";
            bkpDBFull.Devices.AddDevice(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"modeluxe.bak"), DeviceType.File);
            bkpDBFull.BackupSetName = "modeluxe";
            bkpDBFull.BackupSetDescription = "modeluxe database - Full Backup";
            bkpDBFull.ExpirationDate = DateTime.Today.AddDays(360);
            bkpDBFull.Initialize = false;
            bkpDBFull.SqlBackup(myServer);
        }
    }
}