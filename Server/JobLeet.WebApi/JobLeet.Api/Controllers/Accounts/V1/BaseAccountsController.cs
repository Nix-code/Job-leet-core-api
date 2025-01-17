﻿using FluentValidation;
using JobLeet.WebApi.JobLeet.Api.Exceptions;
using JobLeet.WebApi.JobLeet.Api.Logging;
using JobLeet.WebApi.JobLeet.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobLeet.WebApi.JobLeet.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseAccountsController<TEntity, TModel, TRepository> : ControllerBase
        where TEntity : class
        where TModel : class
        where TRepository : IRepository<TEntity, TModel>
    {
        protected readonly TRepository Repository;
        protected readonly ILoggerManagerV1 _logger;
        protected readonly IValidator<TEntity> _validator;

        protected BaseAccountsController(
            TRepository repository,
            ILoggerManagerV1 logger,
            IValidator<TEntity> validator
        )
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        #region Retrieve record GET request
        /// <returns>The list of records.</returns>
        /// <exception cref="Exception">Thrown when there is an error while fetching data from the database.</exception>
        /// <remarks>This method fetches all the records from the database using Entity Framework Core.</remarks>
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.LogInfo("Triggering HTTP GET request");
                var entities = await Repository.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching all entities: {ex.Message}");
                var errorResponse = new GlobalErrorResponse
                {
                    Error = "Internal Server Error",
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }
        #endregion

        #region Retrieve record GET By ID request
        /// <returns>The record by ID.</returns>
        /// <exception cref="Exception">Thrown when there is an error while fetching data from the database.</exception>
        /// <remarks>This method fetches the record by ID from the database using Entity Framework Core.</remarks>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var entity = await Repository.GetByIdAsync(id);

                if (entity == null)
                    return NotFound();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching the data: {ex.Message}");
                var errorResponse = new GlobalErrorResponse
                {
                    Error = "System Exception",
                    Message = ex.Message,
                };
                return StatusCode(400, errorResponse);
            }
        }
        #endregion

        #region POST Request
        /// <returns>The newly created record.</returns>
        /// <exception cref="Exception">Thrown when there is an error while fetching data from the database.</exception>
        /// <remarks>This method posts the record to the database using Entity Framework Core.</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> CreateAsync([FromBody] TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return BadRequest();
                }
                var validationResult = _validator.Validate(entity);
                if (!validationResult.IsValid)
                {
                    return BadRequest(
                        new
                        {
                            Message = "Validation failed.",
                            Errors = validationResult.Errors.Select(e => new
                            {
                                e.PropertyName,
                                e.ErrorMessage,
                            }),
                        }
                    );
                }
                var result = await Repository.AddAsync(entity);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating the entity: {ex.Message}");
                var errorResponse = new GlobalErrorResponse
                {
                    Error = "System Exception",
                    Message = ex.Message,
                };
                return StatusCode(400, errorResponse);
            }
        }
        #endregion
    }
}
