using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Display(Name="姓名")]
        [Required(ErrorMessage ="请输入名字"),MaxLength(50,ErrorMessage ="名字的长度不能超过50个字符")]
        public string Name { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "邮箱的格式不正确")]
        [Required(ErrorMessage = "请输入邮箱地址")]
        public string Email { get; set; }
        [Display(Name = "班级")]
        public ClassNameEnum? ClassName { get; set; }
        public string? PhotoPath { get; set; }


        
    }
}
