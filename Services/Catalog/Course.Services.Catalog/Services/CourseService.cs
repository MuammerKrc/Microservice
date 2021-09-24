using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using Course.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<CourseModel> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService( IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CatalogCollectionName);
            _courseCollection = database.GetCollection<CourseModel>(databaseSettings.CourseCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(x => true).ToListAsync();
            if(courses.Any())
            {
                foreach (var item in courses)
                {
                    if (item.CategoryId != null)
                    {
                        var category = await _categoryCollection.Find(x => x.Id == item.CategoryId).SingleOrDefaultAsync();
                        if (category != null)
                        {
                            item.Category = category;
                        }else
                        {
                            item.Category = new Category();
                        }
                    }
                }
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }
        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).SingleOrDefaultAsync();
            if(course==null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }
            if (course.CategoryId != null)
            {
                var category = await _categoryCollection.Find(x => x.Id == course.CategoryId).SingleOrDefaultAsync();
                if (category != null)
                {
                    course.Category = category;
                }
                else
                {
                    category = new Category();
                }
            }
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(i => i.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    if (item.CategoryId != null)
                    {
                        var category = await _categoryCollection.Find(x => x.Id == item.CategoryId).SingleOrDefaultAsync();
                        if (category != null)
                        {
                            item.Category = category;
                        }
                        else
                        {
                            item.Category = new Category();
                        }
                    }
                }
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }
        public async Task<Response<CourseDto>> CreateCourseASync(CourseCreateDto courseCreate)
        {
            var newCourse = _mapper.Map<CourseModel>(courseCreate);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 201);
        }
        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<CourseModel>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id,updateCourse);
            if(result==null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
            return Response<NoContent>.Success(204);
        } 
        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if(result.DeletedCount>0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
        }

    }
}
