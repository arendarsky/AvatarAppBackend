using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure
{
    public abstract class EfBaseRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected readonly AvatarAppContext DbContext;
        protected readonly IStorageService StorageService;
        protected string StoragePrefix { get; set; } 

        protected EfBaseRepository(AvatarAppContext dbContext, IStorageService storageService)
        {
            DbContext = dbContext;
            StorageService = storageService;
        }

        public T GetById(int id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>()
                .FirstOrDefault(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>()
                .FirstOrDefaultAsync(predicate);
        }

        public T Get(ISpecification<T> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(DbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return secondaryResult
                .FirstOrDefault(spec.Criteria);
        }

        public async Task<T> GetAsync(ISpecification<T> spec)
        {
            var result = AddIncludes(spec);

            return await result
                .FirstOrDefaultAsync(spec.Criteria);
        }


          
        public Stream GetFile(string fileName)
        {
            return StorageService.GetFileStream(fileName, StoragePrefix);
        }

        public void RemoveFiles(ICollection<string> existFiles)
        {
            StorageService.RemoveUnusedFiles(existFiles, StoragePrefix);
        }

        public IEnumerable<T> List()
        {
            return DbContext.Set<T>().AsEnumerable();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>()
                .Where(predicate)
                .AsEnumerable();
        }

        public IEnumerable<T> List(ISpecification<T> spec)
        {
            var result = AddIncludes(spec);

            return result
                .Where(spec.Criteria)
                .AsEnumerable();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>()
                .Count(predicate);
        }

        public void Insert(T entity)
        {
            DbContext.Set<T>().Add(entity);

            DbContext.SaveChanges();
        }

        public async Task InsertAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);

            await DbContext.SaveChangesAsync();
        }

        public async Task InsertFileAsync(IFormFile file, string fileName)
        {
            await StorageService.UploadAsync(file, fileName, StoragePrefix);
        }

        public void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;

            DbContext.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            DbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);

            DbContext.SaveChanges();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var deleted = DbContext.Set<T>().Where(predicate);

            DbContext.Set<T>().RemoveRange(deleted);

            DbContext.SaveChanges();
        }

        private IQueryable<T> AddIncludes(ISpecification<T> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(DbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            return spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));
        }

    }
}
