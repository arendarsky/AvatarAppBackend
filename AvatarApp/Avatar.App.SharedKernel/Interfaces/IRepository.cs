using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Avatar.App.SharedKernel.Interfaces
{
    public interface IRepository<T> where T: BaseEntity
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        T Get(ISpecification<T> spec);
        Task<T> GetAsync(ISpecification<T> spec);
        Stream GetFile(string fileName);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        IEnumerable<T> List(ISpecification<T> spec);
        int Count(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        Task InsertAsync(T entity);
        Task InsertFileAsync(IFormFile file, string fileName);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void RemoveFiles(ICollection<string> existFiles);
    }
}