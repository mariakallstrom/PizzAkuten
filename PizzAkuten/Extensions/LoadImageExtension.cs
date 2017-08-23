using PizzAkuten.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Extensions
{
    public static class LoadImageExtension
    {
        public static string GetPicture(this Dish dish)
        {
            return dish.ImagePath;
        }
    }
}
