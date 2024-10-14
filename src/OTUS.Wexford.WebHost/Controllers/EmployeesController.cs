using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OTUS.Wexford.Core.Abstractions.Repositories;
using OTUS.Wexford.Core.Domain.Administration;
using OTUS.Wexford.WebHost.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace OTUS.Wexford.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IConfiguration _config;
        private readonly DbConfig _dbOptions;
        public EmployeesController(
            IRepository<Employee> employeeRepository,
            IOptions<DbConfig> dbOptions,
            IConfiguration config)
        {
            _employeeRepository = employeeRepository;
            _config = config;
            _dbOptions = dbOptions.Value;

        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var dbLogin1 = _config.GetValue<string>("db:Login");
            var dbLogin2 = _dbOptions.Login;

            var employees = await _employeeRepository.GetAllAsync(cancellationToken);

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponseDto()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return Ok(employeeModel);
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveEmployeeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (employee == null)
                return NotFound();

            await _employeeRepository.RemoveByIdAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Создать сотрудника
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> CreateEmployee([FromBody] CreateEmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            if (employeeDto == null)
                return BadRequest();

            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName
            };

            var newEmployee = await _employeeRepository.AddAsync(employee);

            var dto = new EmployeeResponseDto
            {
                FullName = newEmployee.FullName,
                Email = newEmployee.Email,
                Id = newEmployee.Id
            };

            return Created($"{Request.GetDisplayUrl()}/{dto.Id}", dto);
        }

        /// <summary>
        /// Обновить сотрудника
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> UpdateEmployee(Guid id, [FromBody] CreateEmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            if (employeeDto == null)
                return BadRequest();

            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (employee != null)
            {
                employee.FirstName = employeeDto.FirstName;
                employee.Email = employeeDto.Email;
                employee.LastName = employeeDto.LastName;

                var updatedEmployee = await _employeeRepository.UpdateByIdAsync(id, employee);

                var dto = new EmployeeResponseDto
                {
                    FullName = updatedEmployee.FullName,
                    Email = updatedEmployee.Email,
                    Id = updatedEmployee.Id
                };

                return Ok(dto);
            }

            return NotFound();

        }
    }
}