using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using net_il_mio_fotoalbum.Data;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPhotos()
        {
            return Ok(PhotoManager.GetAllPhotos());
        }

        [HttpGet("{id}")]
        public IActionResult GetPhotoById(int id)
        {
            return Ok(PhotoManager.GetPhotoById());

        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message messaggio)
        {
            SendManager(messaggio);
            return Ok();
        }

        private void SendManager(Message messaggio)
        {
            throw new NotImplementedException();
        }
    }
}
