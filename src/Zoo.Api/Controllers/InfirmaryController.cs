namespace Zoo.Api.Controllers
{
    using System.Collections.Immutable;
    using System.Net;
    using System.Threading.Tasks;

    using Application.Queries;

    using Infirmary.VeterinaryAggregate.Models;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InfirmaryController : ControllerBase
    {
        [HttpGet("veterinaries")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IImmutableList<VeterinaryContact>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Get([FromServices] IGetVeterinariesQuery getVeterinariesQuery)
        {
            ActionResult result = this.StatusCode((int)HttpStatusCode.InternalServerError);
            await getVeterinariesQuery.GetAsync(
                async veterinaryContacts => result = this.Ok(veterinaryContacts),
                () => result = this.StatusCode((int)HttpStatusCode.NoContent));
            return result;
        }
    }
}