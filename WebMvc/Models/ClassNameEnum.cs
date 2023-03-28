using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public enum ClassNameEnum
    {
        [Display(Name ="未分配")]
        None,
        [Display(Name = "一年级")]
        FirstGrade,
        [Display(Name = "二年纪")]
        SecondGrade,
        [Display(Name = "三年纪")]
        GradeThree

    }
}
