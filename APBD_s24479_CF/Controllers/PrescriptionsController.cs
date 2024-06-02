using APBD_s24479_CF.DTOModel;
using APBD_s24479_CF.Model;
using APBD_s24479_CF.Service;//changed
using Microsoft.AspNetCore.Mvc;

namespace APBD_s24479_CF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionsService _prescriptionsService;

        public PrescriptionsController(IPrescriptionsService prescriptionsService)
        {
            _prescriptionsService = prescriptionsService;
        }

        [HttpPost]
        public IActionResult AddPrescription([FromBody] PrescriptionDTO prescription)
        {
            var result = _prescriptionsService.AddPrescription(prescription);
            return Ok(result);
        }
    }
}