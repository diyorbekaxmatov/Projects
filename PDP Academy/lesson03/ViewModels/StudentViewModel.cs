using PDP_Academy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PDP_Academy.ViewModels
{
    public class StudentViewModel
    {
        public List<Student> Students { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public string Group { get; set; }
    }
}
