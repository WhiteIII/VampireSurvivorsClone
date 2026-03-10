using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.VIew.Base;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public class WindowsCreator : LocalObjectCreator<Window>
    {
        public WindowsCreator(
            IRepository<Window> repository, 
            IInstantiator instantiator, 
            LocalAssetProvider localAssetProvider) : base(repository, instantiator, localAssetProvider)
        {
        }
    }
}