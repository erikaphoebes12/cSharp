using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiWeb.Api_Controllers
{
    public class StudentController : ApiController
    {
        Data.DbItElec4DataContext db = new Data.DbItElec4DataContext();

        // GET api<controller>/<course>/5
        [Route("api/student/course/")]
        public List<Api_Models.MstCourse_ApiModel> GetCourse()
        {
            var courses = from d in db.MstCourses
                          select new Api_Models.MstCourse_ApiModel
                          {
                              Id = d.Id,
                              CourseCode = d.CourseCode,
                              Course = d.Course
                          };

            return courses.ToList();
        }

        // GET api/<controller>
        public IEnumerable<Api_Models.MstStudent_ApiModel> Get()
        {
            var students = from d in db.MstStudents
                           select new Api_Models.MstStudent_ApiModel
                           {
                               Id = d.Id,
                               StudentCode = d.StudentCode,
                               FullName = d.FullName,
                               CourseId = d.MstCourse.Id,
                               Course = d.MstCourse.Course
                           };

            return students.OrderBy(d => d.StudentCode).ToList();
        }

        // GET api/<controller>/5
        public Api_Models.MstStudent_ApiModel Get(String studentId)
        {
            var student = from d in db.MstStudents
                          where d.Id == Convert.ToInt32(studentId)
                          select new Api_Models.MstStudent_ApiModel
                          {
                              Id = d.Id,
                              StudentCode = d.StudentCode,
                              FullName = d.FullName,
                              CourseId = d.MstCourse.Id,
                              Course = d.MstCourse.Course
                          };

            return student.FirstOrDefault();
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Api_Models.MstStudent_ApiModel objStudent)
        {
            try
            {

                var course = from d in db.MstCourses
                             where d.Id == objStudent.CourseId
                             select d;

                if (course.Any())
                {
                    Data.MstStudent newStudent = new Data.MstStudent
                    {
                        StudentCode = objStudent.StudentCode,
                        FullName = objStudent.FullName,
                        CourseId = course.FirstOrDefault().Id,
                    };

                    db.MstStudents.InsertOnSubmit(newStudent);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Successfully added!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Course not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(string studentId, Api_Models.MstStudent_ApiModel objStudent)
        {
            try
            {
                var student = from d in db.MstStudents
                              where d.Id == Convert.ToInt32(studentId)
                              select d;

                var course = from d in db.MstCourses
                             where d.Id == objStudent.CourseId
                             select d;

                if (!course.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Course not found!");
                }

                if (student.Any())
                {
                    var updateStudent = student.FirstOrDefault();
                    updateStudent.StudentCode = objStudent.StudentCode;
                    updateStudent.FullName = objStudent.FullName;
                    updateStudent.CourseId = course.FirstOrDefault().Id;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Successfully added!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(string studentId)
        {
            try
            {
                var student = from d in db.MstStudents
                              where d.Id == Convert.ToInt32(studentId)
                              select d;

                if (student.Any())
                {
                    db.MstStudents.DeleteOnSubmit(student.FirstOrDefault());
                    db.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Successfully deleted!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);

            }
        }
    }
}