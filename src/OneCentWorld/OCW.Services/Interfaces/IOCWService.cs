using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using OCW.DAL.DTOs;

namespace OCW.Services.Interfaces
{
    [ServiceContract]
    interface IOCWService
    {
        #region Authenthication

        [OperationContract(Name = "LoginWithName")]
        Guid Login(string user,string pass);

        [OperationContract]
        Guid Login(int userId, string pass);

        #endregion Authenthication

        #region User

        [OperationContract]
        void CreateUser(PersonDTO person);

        [OperationContract]
        PersonDTO GetUser(Guid ticket, int userId);

        [OperationContract(Name = "GetUserWithName")]
        PersonDTO GetUser(Guid ticket, string user);

        [OperationContract]
        void DeleteUser(Guid ticket, int user);

        #endregion User

        #region Accounts

        [OperationContract]
        bool Transfer(Guid ticket, int from, int to, decimal ammount, string[] tags);

        #endregion Accounts

        #region Company

        #endregion Company

        #region Organization

        #endregion Organization

    }
}
