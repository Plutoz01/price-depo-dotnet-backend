using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceDepo.Models;
using PriceDepo.Repositories;

namespace PriceDepo.Controllers
{
	[ApiController]
	public abstract class AbstractCrudController<TEntity, TIdentifier> : ControllerBase
		where TEntity : IIdentifiable<TIdentifier>
	{

		protected readonly ICrudRepository<TEntity, TIdentifier> _repository;

		public AbstractCrudController(ICrudRepository<TEntity, TIdentifier> repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public ActionResult<TEntity[]> GetAll([FromQuery] PaginationParameters pagination)
		{
			return _repository.GetAll(pagination.Limit, pagination.Offset).ToArray();
		}

		[HttpGet("{id}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<TEntity> Get(TIdentifier id)
		{
			var result = _repository.GetById(id);
			if (result == null)
			{
				return NotFound();
			}
			return result;
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public ActionResult<TEntity> CreateOne([FromBody] TEntity newEntity)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (newEntity.Id != null)
			{
				return BadRequest("New entity must not have Id");
			}
			return _repository.Save(newEntity);
		}

		[HttpPost("batch")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public ActionResult<TEntity[]> CreateMany([FromBody] TEntity[] newEntities)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var hasAnyId = newEntities.Any(entity => entity.Id != null);
			if (hasAnyId)
			{
				return BadRequest("New entities must not have Id");
			}

			return _repository.Save(newEntities).ToArray();
		}

		[HttpPut("{id}")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public ActionResult<TEntity> UpdateOne(TIdentifier id, [FromBody] TEntity entity)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (!id.Equals(entity.Id))
			{
				return BadRequest($"mismatching identifiers (path: {id}, body: {entity.Id})");
			}
			if (!_repository.IsExists(id))
			{
				return NotFound();
			}
			return _repository.Save(entity);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public IActionResult Delete(TIdentifier id)
		{
			if (!_repository.IsExists(id))
			{
				return NotFound();
			}
			_repository.Remove(id);
			return NoContent();
		}
	}
}