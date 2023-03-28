using WebMvc.Models;

namespace StudentManagement.Models
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly ILogger<SQLStudentRepository> logger;
        private readonly AppDbContext _context;

        public SQLStudentRepository(AppDbContext context,ILogger<SQLStudentRepository> logger)
        {
            this.logger = logger;
            this._context = context;
        }
        public Student Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);
            if(student != null)
            {
                this._context.Students.Remove(student);
                this._context.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            logger.LogTrace("Trace(跟踪) Log");
            logger.LogDebug("Debug(调试) Log");
            logger.LogInformation("信息(Information）Log");
            logger.LogWarning("警告(Warning) Log");
            logger.LogError("错误(Error) Log");
            logger.LogCritical("严重(Critical) Log");


            return this._context.Students;
        }

        public Student GetStudent(int id)
        {
            return this._context.Students.Find(id);
        }

        public Student Update(Student updateStudent)
        {
            var student = _context.Students.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }
    }
}
