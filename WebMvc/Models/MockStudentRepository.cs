﻿namespace WebMvc.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _studentsList;

        public MockStudentRepository()
        {
            _studentsList = new List<Student>()
            {
                new Student() {Id = 1, Name = "张三", ClassName=ClassNameEnum.FirstGrade, Email="Tony-zhang@52abp.com" },
                new Student() {Id = 2, Name = "李四", ClassName=ClassNameEnum.SecondGrade, Email="lisi@52abp.com" },
                new Student() {Id = 3, Name = "王二麻子", ClassName=ClassNameEnum.GradeThree, Email="wang@52abp.com" }
            };
        }

        public Student Add(Student student)
        {
            student.Id = _studentsList.Max(s => s.Id) + 1;
            _studentsList.Add(student);
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _studentsList.FirstOrDefault(s =>s.Id == id);

            if(student != null)
            {
                _studentsList.Remove(student);
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentsList;
        }

        public Student GetStudent(int id)
        {
            return _studentsList.FirstOrDefault(a => a.Id == id);
        }

        public Student Update(Student updateStudent)
        {
            Student student = _studentsList.FirstOrDefault(s =>s.Id == updateStudent.Id);

            if(student != null)
            {
                student.Name = updateStudent.Name;
                student.Email = updateStudent.Email;
                student.ClassName = updateStudent.ClassName;
            }

            return student;

        }
    }
}
