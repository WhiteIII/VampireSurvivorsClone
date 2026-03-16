using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services.Repositrories;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Implementation
{
    public class WindowsCreator : LocalObjectCreator<Window>
    {
        public WindowsCreator(
            WindowsRepository repository, 
            IInstantiator instantiator, 
            LocalAssetProvider localAssetProvider) : base(repository, instantiator, localAssetProvider)
        {
        }
    }
}