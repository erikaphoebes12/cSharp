using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiWeb.Api_Controllers
{
    public class CourseController : ApiController
    {
        Data.DbItElec4DataContext db = new Data.DbItElec4DataContext();

        // GET api/<controller>
        public IEnumerable<Api_Models.MstCourse_ApiModel> Get()
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

        // GET api/<controller>/5
        public Api_Models.MstCourse_ApiModel Get(int id)
        {
            var courses = from d in db.MstCourses
                          where d.Id == Convert.ToInt32(id)
                          select new Api_Models.MstCourse_ApiModel
                          {
                              Id = d.Id,
                              CourseCode = d.CourseCode,
                              Course = d.Course
                          };

            return courses.FirstOrDefault();
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Api_Models.MstCourse_ApiModel objCourse)
        {
            try
            {
                Data.MstCourse newCourse = new Data.MstCourse
                {
                    CourseCode = objCourse.CourseCode,
                    Course = objCourse.Course
                };
                db.MstCourses.InsertOnSubmit(newCourse);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Api_Models.MstCourse_ApiModel objCourse, String Id)
        {
            try
            {
                var course = from d in db.MstCourses
                             where d.Id == Convert.ToInt32(Id)
                             select d;

                if (course.Any())
                {
                    var updateCourse = course.FirstOrDefault();
                    updateCourse.CourseCode = objCourse.CourseCode;
                    updateCourse.Course = objCourse.Course;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Course not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(String Id)
        {
            try
            {
                var course = from d in db.MstCourses
                             where d.Id == Convert.ToInt32(Id)
                             select d;

                if (course.Any())
                {
                    db.MstCourses.DeleteOnSubmit(course.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
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
    }
}