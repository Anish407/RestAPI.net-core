using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
        }


        [HttpGet()]
        // if the search and filter are empty then we return all the results else 
        // we convert the query to an IQueryable and return the filtered results
        [HttpHead]
        // to check if the endpoint is accessible
        // we just need to add this to any endpoint and when we call it 
        // the code inside will get executed but the response body will be null.
        public IActionResult GetAuthors(string mainCategory, string searchQuery)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(mainCategory, searchQuery);
            return Ok(authorsFromRepo);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(authorFromRepo);
        }

        [HttpPost]
        //we need to create a separate model for this endpoint, as it doesnt need an 
        //id field in its input. So if there are other fields that wont be sent then
        // create a model that has only the properties that will be sent to this endpoint
        public async Task<IActionResult> CreateAuthor([FromBody]CreateAuthorDTO model)
        {
            //convert to entity
            var newAuthor = new Entities.Author
            {
                DateOfBirth = model.DOB,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MainCategory = model.MainCategory
            };

            // also contains method to add collection of courses for the author
            await _courseLibraryRepository.AddAuthorAsync(newAuthor);
            _courseLibraryRepository.Save();

            // Map to return type DTO
            var authorDto = new AuthorDto
            {
                Id = newAuthor.Id,
                Name=$"{newAuthor.FirstName} {newAuthor.LastName}",
                Age = DateTime.Now.Year - newAuthor.DateOfBirth.Year,
                MainCategory = newAuthor.MainCategory
            };

            // route name is the name of the route that will return details for the 
            //newly created author ()
            //routeValues: we need to create an object with the same arg names as the 
            // route args for GetAuthor
            // value: will be the newly created author object

            // this will return the author.. to get its corresponding courses
            // append /Courses to the route
            return CreatedAtRoute(routeName:"GetAuthor",
                routeValues: new { authorId = authorDto.Id }, 
                value:authorDto);
        }
    }
}
