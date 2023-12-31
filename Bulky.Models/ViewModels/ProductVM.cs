﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
    //This View Model exists to pass information from a
    //Product Model and Category Model without using
    //ViewBag or ViewData.
    public class ProductVM
    {
        //This prop will contain the Product object 
        //for the view.
        public Product Product { get; set; }

        //This prop will contain what we want to use
        //in the dropdown.
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
