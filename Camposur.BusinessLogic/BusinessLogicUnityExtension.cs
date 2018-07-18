using Camposur.BusinessLogic.Logic.Interfaces;
using Camposur.DataAccess;
using Microsoft.Practices.Unity;

namespace Camposur.BusinessLogic
{
    public class BusinessLogicUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.AddNewExtension<DataAccessUnityExtension>();
            Container.RegisterType<IAuthLogic, AuthLogic>();
            Container.RegisterType<ISendGridLogic, SendGridLogic>();
        }
    }
}
