﻿using Smartstore.Core.Catalog.Brands;
using Smartstore.Core.Catalog.Categories;
using Smartstore.Core.Catalog.Discounts;
using Smartstore.Core.Catalog.Products;

namespace Smartstore.Web.Api.Controllers.OData
{
    /// <summary>
    /// The endpoint for operations on Discount entity.
    /// </summary>
    public class DiscountsController : WebApiController<Discount>
    {
        [HttpGet, ApiQueryable]
        [Permission(Permissions.Promotion.Discount.Read)]
        public IQueryable<Discount> Get()
        {
            return Entities.AsNoTracking();
        }

        [HttpGet, ApiQueryable]
        [Permission(Permissions.Promotion.Discount.Read)]
        public SingleResult<Discount> Get(int key)
        {
            return GetById(key);
        }

        [HttpGet, ApiQueryable]
        [Permission(Permissions.Promotion.Discount.Read)]
        public IQueryable<Category> GetAppliedToCategories(int key)
        {
            return GetRelatedQuery(key, x => x.AppliedToCategories);
        }

        [HttpGet, ApiQueryable]
        [Permission(Permissions.Promotion.Discount.Read)]
        public IQueryable<Manufacturer> GetAppliedToManufacturers(int key)
        {
            return GetRelatedQuery(key, x => x.AppliedToManufacturers);
        }

        [HttpGet, ApiQueryable]
        [Permission(Permissions.Promotion.Discount.Read)]
        public IQueryable<Product> GetAppliedToProducts(int key)
        {
            return GetRelatedQuery(key, x => x.AppliedToProducts);
        }

        [HttpPost]
        [Permission(Permissions.Promotion.Discount.Create)]
        public async Task<IActionResult> Post([FromBody] Discount entity)
        {
            return await PostAsync(entity);
        }

        [HttpPut]
        [Permission(Permissions.Promotion.Discount.Update)]
        public async Task<IActionResult> Put(int key, Delta<Discount> model)
        {
            return await PutAsync(key, model);
        }

        [HttpPatch]
        [Permission(Permissions.Promotion.Discount.Update)]
        public async Task<IActionResult> Patch(int key, Delta<Discount> model)
        {
            return await PatchAsync(key, model);
        }

        [HttpDelete]
        [Permission(Permissions.Promotion.Discount.Delete)]
        public async Task<IActionResult> Delete(int key)
        {
            return await DeleteAsync(key);
        }
    }
}
