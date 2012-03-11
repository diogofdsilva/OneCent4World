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
    public class RoleDAO : IRoleDAO, IRoleFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public RoleDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IRoleDAO Implementation
        public Role Add(Role entity)
        {
            if (entity.Id > 0) return entity;

            context.Role.AddObject(entity);

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

        public Role Read(int key)
        {
            var listRoles = context.Role.Where(r => key == r.Id);
            if (listRoles.Count() == 0) throw new RecordNotFoundException<int?>("Role", key);
            Role role = listRoles.First();

            return role;
        }

        public IEnumerable<Role> ReadAll(Func<Role, bool> predicate = null)
        {
            return context.Role.Where(predicate ?? (p => true));
        }

        public IEnumerable<Role> ReadAll(int pageSize, int page, Func<Role, bool> predicate)
        {
            return context.Role.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Role, bool> predicate)
        {
            return context.Role.Where(predicate ?? (p => true)).Count();
        }

        public Role Update(Role entity)
        {
            return entity;
        }

        public void Delete(Role entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var roleList = context.Role.Where(r => r.Id == entity);
            if (roleList.Count() == 0) throw new RecordNotFoundException<int>("Role", entity);
            var role = roleList.First();

            context.DeleteObject(role);
        }

        public Role FindByName(string name)
        {
            var nameList = context.Role.Where(r => r.Name.ToLower().Equals(name.ToLower()));
            if (nameList.Count() == 0) throw new FieldNotFoundException<string>("Role", name, "name");

            return nameList.First();
        }

        #endregion

        #region IRoleFactory Implementation
        public Role Create(int id, string name)
        {
            Role r = context.CreateObject<Role>();
            r.Id = id;
            r.Name = name;
            return r;
        }
        #endregion
    }
}
