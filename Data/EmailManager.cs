using Microsoft.AspNetCore.Mvc.ViewFeatures;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class EmailManager
    {
        public static void InviaEmail(Email email)
        {
            using PhotoContext db = new PhotoContext();
            db.Emails.Add(email);
            db.SaveChanges();
        }
    }
}
