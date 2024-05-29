using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class MessageManager
    {
        public static void InviaMessaggio(Models.Message messaggio)
        {
            using PhotoContext db = new PhotoContext();

            db.Messages.Add(messaggio);
            db.SaveChanges();
        }
    }

}
