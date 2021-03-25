namespace Zoo.Api.Controllers
{
    using System.Collections.Immutable;
    using System.Net;
    using System.Threading.Tasks;

    using Administration.Common;

    using Application.Commands;
    using Application.Queries;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Park.Common.Models;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AnimalsController<TDetails, TRestrained, TCreating> : ControllerBase
        where TDetails : AnimalDetails where TRestrained : AnimalRestrained, new() where TCreating : AnimalCreating
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IImmutableList<>), 200)]
        [ProducesResponseType(500)]
        public ActionResult Get([FromServices] IGetRestrainedAnimalsQuery getRestrainedAnimalsQuery)
        {
            ActionResult result = this.StatusCode((int)HttpStatusCode.InternalServerError);
            getRestrainedAnimalsQuery.Get<TRestrained>(
                animals => result = this.Ok(animals),
                () => result = this.StatusCode((int)HttpStatusCode.NoContent));
            return result;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AnimalDetails), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post(
            [FromBody] TCreating createAnimal,
            [FromServices] IAnimalRegistrationCommand animalRegistrationCommand)
        {
            ActionResult result = this.StatusCode((int)HttpStatusCode.InternalServerError);
            await animalRegistrationCommand.ExecuteAsync<TCreating, TDetails>(
                createAnimal,
                createdAnimal => result = this.Created(
                                     $"api/v{this.RouteData.Values["version"]}/{this.RouteData.Values["controller"]}/{createdAnimal.Id}",
                                     createdAnimal),
                errorAnimal => result = this.BadRequest(errorAnimal));
            return result;
        }
    }
}