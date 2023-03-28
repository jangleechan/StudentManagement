using Microsoft.EntityFrameworkCore;
using WebMvc.Models;

namespace StudentManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new Student
            {
                Id = 1,
                Name = "梁同明",
                ClassName = ClassNameEnum.FirstGrade,
                Email = "ltm@ddxc.org"
            },
            new Student
            {
                Id = 2,
                Name = "角落的白板报",
                ClassName = ClassNameEnum.FirstGrade,
                Email = "werltm@qq.com"
            }
            );
        }
    }
}
