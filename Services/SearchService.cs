using Microsoft.EntityFrameworkCore;
using LostAndFound.API.Models;
using LostAndFound.API.Data;

namespace LostAndFound.API.Services
{
    public interface ISearchService
    {
        IQueryable<Item> ApplyFilters(IQueryable<Item> query, SearchFilters filters);
    }

    public class SearchService : ISearchService
    {
        public IQueryable<Item> ApplyFilters(IQueryable<Item> query, SearchFilters filters)
        {
            if (!string.IsNullOrEmpty(filters.SearchTerm))
            {
                query = query.Where(i =>
                    i.Description.Contains(filters.SearchTerm) ||
                    i.Location.Contains(filters.SearchTerm));
            }

            if (filters.Status?.Any() == true)
            {
                query = query.Where(i => filters.Status.Contains(i.Status));
            }

            if (filters.Categories?.Any() == true)
            {
                query = query.Where(i => filters.Categories.Contains(i.Category));
            }

            return query;
        }
    }
}