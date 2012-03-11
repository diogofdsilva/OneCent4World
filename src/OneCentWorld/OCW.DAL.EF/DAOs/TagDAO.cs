using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OCW.DAL.DAOs;
using OCW.DAL.DTOs;
using OCW.DAL.DTOs.Factory;
using OCW.DAL.Exceptions;

namespace OCW.DAL.EF.DAOs
{
    public class TagDAO : ITagDAO, ITagFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public TagDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region ITagDAO Implementation
        public Tag Add(Tag entity)
        {
            if (entity.Id > 0) return entity;

            context.Tag.AddObject(entity);

            try
            {
                context.SaveChanges();
            }
            catch (OptimisticConcurrencyException e)
            {
                //An optimistic concurrency violation has occurred in the data source.
                throw new ConcurrencyException(e.Message);
            }

            return entity;
        }

        public Tag Read(int key)
        {
            var listTag = context.Tag.Where(t => key == t.Id);
            if(listTag.Count() == 0) throw new RecordNotFoundException<int?>("Tag", key);
            Tag tag = listTag.First();

            return tag;
        }

        public IEnumerable<Tag> ReadAll(Func<Tag, bool> predicate = null)
        {
            return context.Tag.Where(predicate ?? (p => true));
        }

        public IEnumerable<Tag> ReadAll(int pageSize, int page, Func<Tag, bool> predicate)
        {
            return context.Tag.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Tag, bool> predicate)
        {
            return context.Tag.Where(predicate ?? (p => true)).Count();
        }

        public Tag Update(Tag entity)
        {
            return entity;
        }

        public void Delete(Tag entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var organizationTag = context.Tag.Where(t => t.Id == entity);
            if (organizationTag.Count() == 0) throw new RecordNotFoundException<int>("Tag", entity);
            var tag = organizationTag.First();

            context.DeleteObject(tag);
        }

        #endregion

        #region ITagFactory Implementation
        public Tag Create(int id, string name)
        {
            Tag ot = context.CreateObject<Tag>();
            ot.Id = id;
            ot.Name = name;
            return ot;
        }
        #endregion
    }
}
