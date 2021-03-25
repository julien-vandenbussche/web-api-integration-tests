namespace Zoo.Api.Controllers
{
    using System.Collections.Immutable;
    using System.Net;

    using Application.Queries;

    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/{animal}/[controller]")]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IImmutableList<>), 200)]
        [ProducesResponseType(500)]
        public ActionResult Get([FromServices] IGetBooksAboutQuery getBooksAboutQuery, [FromRoute] string animal)
        {
            ActionResult result = this.StatusCode((int)HttpStatusCode.InternalServerError);
            getBooksAboutQuery.GetAsync(animal,
                books => result = this.Ok(books),
                () => result = this.StatusCode((int)HttpStatusCode.NoContent));
            return result;
        }   
    }
}