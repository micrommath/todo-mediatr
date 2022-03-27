﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.API.ViewModels;
using Todo.Application.Commands;
using Todo.Application.Queries;

namespace Todo.API.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoViewModel>>> Get()
        {
            var todos = await _mediator.Send(new GetAllTodosQuery());

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoViewModel>> GetById(Guid id)
        {
            var todo = await _mediator.Send(new GetTodoByIdQuery(id));

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult> Add(string title, string description, List<string> tags)
        {
            bool success = await _mediator.Send(new AddTodoCommand(title, description, tags));
            if (!success)
                return BadRequest();

            return Ok();
        }

        [HttpPut("done")]
        public async Task<ActionResult> MarkAsDone(Guid id)
        {
            bool success = await _mediator.Send(new MarkTodoAsDoneCommand(id));
            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}