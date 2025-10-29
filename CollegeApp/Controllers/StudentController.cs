using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(policyName:"MyTestCOR")]
    [Authorize(AuthenticationSchemes = "LoginForLocals", Roles = "SuperAdmin")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
       
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        public StudentController(ILogger<StudentController> logger,  IMapper mapper, IStudentRepository studentRepository)
        {
            _logger = logger;
         
            _mapper = mapper;
            _studentRepository = studentRepository;
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>>  GetStudents()
        {
            _logger.LogInformation("GetStudents method started");
            var students = await _studentRepository.GetAllAsync();

            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
            //var students = await _dbContext.Students.Select(s => new StudentDTO()
            //{
            //    Id = s.Id,
            //    StudentName = s.StudentName,
            //    Address = s.Address,
            //    Email = s.Email,
            //    DOB = s.DOB.ToShortDateString()
                
            //}).ToListAsync();
            //OK - 200 - Success
            return Ok(studentDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>>  GetStudentById(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var student = await _studentRepository.GetByIdAsync(x=>x.Id==id);
            //NotFound - 404 - NotFound - Client error
            if (student == null)
            {
                _logger.LogError("Student not found with given Id");
                return NotFound($"The student with id {id} not found");
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);
            //OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = await _studentRepository.GetByNameAsync(x=>x.StudentName.ToLower().Contains(name.ToLower()));
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with name {name} not found");
            var studentDTO = _mapper.Map<StudentDTO>(student);
            //OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        //api/student/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if (model == null)
                return BadRequest();




            Student student = _mapper.Map<Student>(model);
            //{
               
            //    StudentName = model.StudentName,
            //    Address = model.Address,
            //    Email = model.Email,
            //    DOB = Convert.ToDateTime(model.DOB)
            //};
            var currentStudent = await _studentRepository.CreateAsync(student);

           
            //Status - 201
             
            //New student details
            return CreatedAtRoute("GetStudentById", new { id = currentStudent.Id},currentStudent);
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null || studentDTO.Id <= 0)
                BadRequest();

            var existingStudent = await _studentRepository.GetByIdAsync(x=>x.Id== studentDTO.Id,true);

            if (existingStudent == null)
                return NotFound();
            var newRecord =  _mapper.Map<Student>(studentDTO);
            await _studentRepository.UpdateAsync(newRecord);
            

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existingStudent = await _studentRepository.GetByIdAsync(x=>x.Id== id, true);

            if (existingStudent == null)
                return NotFound();

            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);
            //var studentDTO = new StudentDTO
            //{
            //    Id = existingStudent.Id,
            //    StudentName = existingStudent.StudentName,
            //    Email = existingStudent.Email,
            //    Address = existingStudent.Address
            //};

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            existingStudent = _mapper.Map<Student>(studentDTO);
            //existingStudent.StudentName = studentDTO.StudentName;
            //existingStudent.Email = studentDTO.Email;
            //existingStudent.Address = studentDTO.Address;
            await _studentRepository.UpdateAsync(existingStudent);
            //204 - NoContent
            return NoContent();
        }


        [HttpDelete("Delete/{id}", Name = "DeleteStudentById")]
        //api/student/delete/1
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
                return BadRequest();

            var student = await _studentRepository.GetByIdAsync(x=>x.Id ==id, true);
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with id {id} not found");

            await _studentRepository.DeleteAsync(student);

            //OK - 200 - Success
            return Ok(true);
        }
    }
}
