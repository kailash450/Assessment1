using Assessment1_1.Data;
using Assessment1_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assessment1_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(Guid id) 
        {
            var emp = _employeeRepository.GetEmployeeById(id);
            if (emp is null)
            {
                return NotFound();
            }
            return Ok(emp);
        }
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            _employeeRepository.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployee),new { Id = employee.Id },employee);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee object is null");
            }

            // Ensure the URL ID matches the employee's ID
            //if (id != employee.Id)
            //{
            //    return BadRequest("Employee ID mismatch");
            //}

            // Check if the employee exists
            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Update the employee details
            _employeeRepository.UpdateEmployee(employee);
            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var emp = _employeeRepository.GetEmployeeById(id);
            if(emp is null)
            {
                return NotFound();
            }
            _employeeRepository.DeleteEmployee(id);
            return Ok();
        }
    }
}
