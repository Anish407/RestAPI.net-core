using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{id}/Courses")]
    public class AuthorCoursesController : ControllerBase
    {

        //this will return all the courses for an author 
        [HttpGet]
        public IActionResult GetCoursesForAuthor(string id, 
                     [FromQuery]string searchParam, 
                     [FromServices] ICourseLibraryRepository courseLibraryRepository)
        {
            return Ok();
        }

        [HttpGet("{courseId}")]
        public IActionResult GetCourseByCOurseIdForAuthor(string id, string courseId)
        {
            return Ok();
        }
    }
}