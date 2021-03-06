﻿using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Models
{
    public class CreateAuthorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DOB { get; set; }
        public string MainCategory { get; set; }
        public ICollection<CourseForCreationDto> Courses { get; set; }
        = new List<CourseForCreationDto>();
    }
}
