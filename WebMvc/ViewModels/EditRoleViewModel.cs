using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    public class EditRoleViewModel
    {
        [Display(Name = "角色ID")]
        public string Id { get; set; }
        [Required]
        [Display(Name ="角色名称")]
        public string RoleName { get; set; }   
        
        public List<string> Users { get; set; }

    }
}
